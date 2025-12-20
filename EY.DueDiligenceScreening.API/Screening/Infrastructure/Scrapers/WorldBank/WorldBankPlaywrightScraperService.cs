using Microsoft.Playwright;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Queries;
using EY.DueDiligenceScreening.API.Screening.Domain.Services;
using System.Text.Json;

namespace EY.DueDiligenceScreening.API.Screening.Infrastructure.Scrapers;

public class WorldBankPlaywrightScraperService : IWorldBankScraperService
{
    private const string WORLD_BANK_URL = "https://projects.worldbank.org/en/projects-operations/procurement/debarred-firms";
    private const string GRID_SELECTOR = "#k-debarred-firms";
    private const string GRID_ROWS_SELECTOR = "#k-debarred-firms .k-grid-content tbody tr";
    private const string SEARCH_INPUT_SELECTOR = "#category";

    public async Task<List<WorldBankDebarredItem>> ScrapeAsync(GetInfoByCompanyNameQuery query, CancellationToken cancellationToken = default)
    {
        var companyName = query.companyName;
        
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name cannot be empty", nameof(companyName));

        var results = new List<WorldBankDebarredItem>();
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
            
            await page.GotoAsync(WORLD_BANK_URL, new PageGotoOptions 
            { 
                WaitUntil = WaitUntilState.DOMContentLoaded,
                Timeout = 60000
            });
            cancellationToken.ThrowIfCancellationRequested();
            
            try
            {
                await page.WaitForSelectorAsync(GRID_SELECTOR, 
                    new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                
                await page.WaitForSelectorAsync(GRID_ROWS_SELECTOR, 
                    new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                
            }
            catch (TimeoutException)
            {
                return results;
            }
            
            cancellationToken.ThrowIfCancellationRequested();
            
            await Task.Delay(1000, cancellationToken);
            var initialRowCount = await page.Locator(GRID_ROWS_SELECTOR).CountAsync();
            var searchInput = page.Locator(SEARCH_INPUT_SELECTOR);
            await searchInput.ClickAsync();
            await Task.Delay(200, cancellationToken);
            await searchInput.ClearAsync();
            await Task.Delay(200, cancellationToken);
            
            cancellationToken.ThrowIfCancellationRequested();
            
            await searchInput.PressSequentiallyAsync(companyName, new LocatorPressSequentiallyOptions { Delay = 50 });
            
            await page.EvaluateAsync(@"(searchTerm) => {
                const input = document.getElementById('category');
                if (input) {
                    input.dispatchEvent(new Event('input', { bubbles: true }));
                    input.dispatchEvent(new Event('change', { bubbles: true }));
                    input.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true, key: 'Enter' }));
                }
            }", companyName);

            await Task.Delay(3000, cancellationToken); 
            cancellationToken.ThrowIfCancellationRequested();
            
            var rowCount = await page.Locator(GRID_ROWS_SELECTOR).CountAsync();

            if (rowCount == 0)
            {
                return results;
            }
            
            if (rowCount == initialRowCount)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                await page.EvaluateAsync(@"(searchTerm) => {
                    const input = document.getElementById('category');
                    if (input) {
                        input.value = searchTerm;
                        if (typeof jQuery !== 'undefined') {
                            jQuery(input).trigger('input');
                            jQuery(input).trigger('change');
                            jQuery(input).trigger('keyup');
                        }
                        input.dispatchEvent(new Event('input', { bubbles: true }));
                        input.dispatchEvent(new Event('change', { bubbles: true }));
                    }
                }", companyName);
                
                await Task.Delay(3000, cancellationToken);
                rowCount = await page.Locator(GRID_ROWS_SELECTOR).CountAsync();
            }

            cancellationToken.ThrowIfCancellationRequested();

            var jsonResponse = await page.EvaluateAsync<JsonElement>(@"() => {
                const rows = Array.from(document.querySelectorAll('#k-debarred-firms .k-grid-content tbody tr'));
                const results = [];
                for (const row of rows) {
                    const cells = row.querySelectorAll('td');
                    if (cells.length < 7) continue;
                    results.push({
                        firmName: cells[0].innerText.trim(),
                        address: cells[2].innerText.trim(),
                        country: cells[3].innerText.trim(),
                        fromDate: cells[4].innerText.trim(),
                        toDate: cells[5].innerText.trim(),
                        grounds: cells[6].innerText.trim()
                    });
                }
                return results;
            }");

            if (jsonResponse.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in jsonResponse.EnumerateArray())
                {
                    if (results.Count % 10 == 0)
                        cancellationToken.ThrowIfCancellationRequested();

                    string GetValue(string prop) => 
                        item.TryGetProperty(prop, out var v) ? v.GetString() ?? "" : "";

                    results.Add(new WorldBankDebarredItem(
                        firmName: GetValue("firmName"),
                        address: string.IsNullOrWhiteSpace(GetValue("address")) ? "N/A" : GetValue("address"),
                        country: string.IsNullOrWhiteSpace(GetValue("country")) ? "Unknown" : GetValue("country"),
                        fromDate: GetValue("fromDate"),
                        toDate: GetValue("toDate"),
                        grounds: GetValue("grounds")
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
           
            throw new InvalidOperationException($"Failed to scrape World Bank for company: {companyName}", ex);
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