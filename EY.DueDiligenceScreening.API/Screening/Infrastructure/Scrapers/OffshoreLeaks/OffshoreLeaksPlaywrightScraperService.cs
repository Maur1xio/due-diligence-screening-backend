using Microsoft.Playwright;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Queries;
using EY.DueDiligenceScreening.API.Screening.Domain.Services;
using System.Text.Json;

namespace EY.DueDiligenceScreening.API.Screening.Infrastructure.Scrapers;

public class OffshoreLeaksPlaywrightScraperService : IOffshoreLeaksScraperService
{
    private readonly ILogger<OffshoreLeaksPlaywrightScraperService> _logger;

    private const string BASE_URL = "https://offshoreleaks.icij.org";
    private const string SEARCH_URL = "https://offshoreleaks.icij.org/search";
    
    public OffshoreLeaksPlaywrightScraperService(ILogger<OffshoreLeaksPlaywrightScraperService> logger)
    {
        _logger = logger;
    }

    public async Task<List<OffshoreLeaksItem>> ScrapeAsync(GetInfoByCompanyNameQuery query)
    {
        var companyName = query.companyName;
        
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name cannot be empty", nameof(companyName));

        var results = new List<OffshoreLeaksItem>();

        try
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions 
            { 
                Headless = true 
            });
            await using var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome Safari/537.36",
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
            });
            var page = await context.NewPageAsync();
            var searchUrl = $"{SEARCH_URL}?q={Uri.EscapeDataString(companyName)}&c=&j=&d=";
            await page.GotoAsync(searchUrl, new PageGotoOptions 
            { 
                WaitUntil = WaitUntilState.NetworkIdle,
                Timeout = 60000
            });
            await Task.Delay(1500);
            try
            {
                var modalExists = await page.Locator(".modal-content").CountAsync() > 0;
                
                if (modalExists)
                {
                    await page.EvaluateAsync(@"() => {
                        const checkbox = document.querySelector('.modal-content input#accept, .modal-content input[name=""accept""]');
                        if (checkbox) {
                            checkbox.checked = true;
                            checkbox.dispatchEvent(new Event('change', { bubbles: true }));
                            checkbox.dispatchEvent(new Event('input', { bubbles: true }));
                        }
                        setTimeout(() => {
                            const submitBtn = document.querySelector('.modal-content button[type=""submit""]');
                            if (submitBtn) {
                                submitBtn.disabled = false;
                                submitBtn.click();
                            }
                        }, 300);
                    }");
                    await Task.Delay(1000);
                    try
                    {
                        await page.WaitForSelectorAsync(".modal-content", 
                            new PageWaitForSelectorOptions { State = WaitForSelectorState.Hidden, Timeout = 10000 });
                    }
                    catch (TimeoutException)
                    {
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }

            try
            {
                await page.WaitForSelectorAsync(".table-responsive, .alert, .no-results, .search__results",
                    new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
            }
            catch (TimeoutException)
            {
                var pageContent = await page.EvaluateAsync<string>(@"() => {
                    const modal = document.querySelector('.modal-content');
                    const table = document.querySelector('.table-responsive');
                    const alert = document.querySelector('.alert');
                    return JSON.stringify({
                        hasModal: !!modal,
                        modalVisible: modal ? modal.offsetParent !== null : false,
                        hasTable: !!table,
                        hasAlert: !!alert,
                        bodyClasses: document.body.className
                    });
                }");
                return results;
            }
            var tableExists = await page.Locator(".search__results__table").CountAsync() > 0;
            if (!tableExists)
            {
                return results;
            }
            var jsonResponse = await page.EvaluateAsync<JsonElement>(@"() => {
                const rows = Array.from(document.querySelectorAll('.search__results__table tbody tr'));
                const results = [];
                
                for (const row of rows) {
                    const cells = row.querySelectorAll('td');
                    if (cells.length !== 4) continue;
                    
                    const entityLink = cells[0].querySelector('a');
                    const sourceLink = cells[3].querySelector('a');
                    
                    results.push({
                        entityName: entityLink ? entityLink.innerText.trim() : '',
                        entityNodeUrl: entityLink ? entityLink.getAttribute('href') : '',
                        jurisdiction: cells[1].innerText.trim(),
                        linkedTo: cells[2].innerText.trim(),
                        dataSource: sourceLink ? (sourceLink.getAttribute('title') || sourceLink.innerText.trim()) : '',
                        dataSourceUrl: sourceLink ? sourceLink.getAttribute('href') : ''
                    });
                }
                
                return results;
            }");
            if (jsonResponse.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in jsonResponse.EnumerateArray())
                {
                    string GetValue(string prop) => 
                        item.TryGetProperty(prop, out var v) ? v.GetString() ?? "" : "";

                    var entityNodeUrl = GetValue("entityNodeUrl");
                    if (!string.IsNullOrEmpty(entityNodeUrl) && entityNodeUrl.StartsWith("/"))
                    {
                        entityNodeUrl = $"{BASE_URL}{entityNodeUrl}";
                    }

                    results.Add(new OffshoreLeaksItem(
                        entityName: GetValue("entityName"),
                        entityNodeUrl: entityNodeUrl,
                        jurisdiction: GetValue("jurisdiction"),
                        linkedTo: string.IsNullOrWhiteSpace(GetValue("linkedTo")) ? "Not identified" : GetValue("linkedTo"),
                        dataSource: GetValue("dataSource"),
                        dataSourceUrl: GetValue("dataSourceUrl")
                    ));
                }
            }
            return results;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to scrape Offshore Leaks for company: {companyName}", ex);
        }
    }
}