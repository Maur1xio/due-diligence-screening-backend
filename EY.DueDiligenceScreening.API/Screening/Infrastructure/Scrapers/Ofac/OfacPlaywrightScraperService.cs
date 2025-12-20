using Microsoft.Playwright;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Queries;
using EY.DueDiligenceScreening.API.Screening.Domain.Services;
using System.Text.Json;

namespace EY.DueDiligenceScreening.API.Screening.Infrastructure.Scrapers;

public class OfacPlaywrightScraperService : IOfacScraperService
{
    private const string OFAC_URL = "https://sanctionssearch.ofac.treas.gov/";
    private const string NAME_INPUT_ID = "#ctl00_MainContent_txtLastName";
    private const string SEARCH_BUTTON_ID = "#ctl00_MainContent_btnSearch";
    private const string RESULTS_TABLE_ID = "#gvSearchResults";

    public async Task<List<OfacItem>> ScrapeAsync(GetInfoByCompanyNameQuery query, CancellationToken cancellationToken = default)
    {
        var companyName = query.companyName;
        
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name cannot be empty", nameof(companyName));

        int minimumScore = 95;
        var results = new List<OfacItem>();

        IBrowser? browser = null;

        try
        {
            using var playwright = await Playwright.CreateAsync();
            
            cancellationToken.ThrowIfCancellationRequested();
            
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions 
            { 
                Headless = true 
            });

            using var cancellationRegistration = cancellationToken.Register(() =>
            {
                try { browser?.CloseAsync().GetAwaiter().GetResult(); } catch { }
            });

            await using var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome Safari/537.36",
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
            });

            var page = await context.NewPageAsync();

            cancellationToken.ThrowIfCancellationRequested();

            await page.GotoAsync(OFAC_URL, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
            await page.WaitForSelectorAsync(NAME_INPUT_ID);

            try
            {
                await page.WaitForFunctionAsync("() => typeof $find !== 'undefined'", 
                    new PageWaitForFunctionOptions { Timeout = 3000 });
            }
            catch (TimeoutException)
            {
            }

            await page.EvaluateAsync(@"(args) => {
                const { minimumScore, companyName } = args;
                
                try {
                    let slider = $find('Slider1');
                    if (slider) {
                        slider.set_Value(minimumScore);
                    }
                } catch(e) { }
                let scoreInput = document.getElementById('ctl00_MainContent_Slider1_Boundcontrol');
                if (scoreInput) {
                    scoreInput.value = minimumScore;
                    scoreInput.dispatchEvent(new Event('change', { bubbles: true }));
                }
                let nameInput = document.getElementById('ctl00_MainContent_txtLastName');
                if (nameInput) {
                    nameInput.value = companyName;
                }
            }", new { minimumScore, companyName });

            cancellationToken.ThrowIfCancellationRequested();

            await page.Locator(SEARCH_BUTTON_ID).ClickAsync();
            
            cancellationToken.ThrowIfCancellationRequested();
            
            try
            {
                await page.WaitForSelectorAsync($"{RESULTS_TABLE_ID}, #ctl00_MainContent_lblMessage", 
                    new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 10000 });
            }
            catch (TimeoutException)
            {
                return results;
            }
            
            cancellationToken.ThrowIfCancellationRequested();
            
            if (await page.Locator(RESULTS_TABLE_ID).CountAsync() == 0)
            {
                return results;
            }
            
            var jsonResponse = await page.EvaluateAsync<JsonElement>(@"() => {
                const rows = Array.from(document.querySelectorAll('#gvSearchResults tbody tr'));
                const list = [];
                
                for (const row of rows) {
                    const cells = row.querySelectorAll('td');
                    if (cells.length !== 6) continue;
                    
                    const nameCell = cells[0];
                    const link = nameCell.querySelector('a');
                    
                    list.push({
                        name: link ? link.innerText.trim() : nameCell.innerText.trim(),
                        detailsUrl: link ? link.href : null,
                        address: cells[1].innerText.trim().replace(/\u00A0/g, ' '),
                        type: cells[2].innerText.trim(),
                        programs: cells[3].innerText.trim(),
                        list: cells[4].innerText.trim(),
                        score: cells[5].innerText.trim()
                    });
                }
                return list;
            }");

            if (jsonResponse.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in jsonResponse.EnumerateArray())
                {
                    if (results.Count % 10 == 0)
                        cancellationToken.ThrowIfCancellationRequested();

                    string GetValue(string prop) => item.TryGetProperty(prop, out var v) ? v.GetString() ?? "" : "";

                    string scoreText = GetValue("score");
                    _ = int.TryParse(scoreText, out int score);

                    results.Add(new OfacItem(
                        name: GetValue("name"),
                        address: string.IsNullOrWhiteSpace(GetValue("address")) ? "N/A" : GetValue("address"),
                        type: GetValue("type"),
                        programs: GetValue("programs"),
                        list: GetValue("list"),
                        score: score,
                        detailsUrl: GetValue("detailsUrl")
                    ));
                }
            }

            return results;
        }
        catch (OperationCanceledException)
        {
            throw; 
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to scrape OFAC for company: {companyName}", ex);
        }
        finally
        {
            if (browser != null)
            {
                try { await browser.CloseAsync(); } catch { }
            }
        }
    }
}