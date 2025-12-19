using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Queries;
using EY.DueDiligenceScreening.API.Screening.Domain.Services;

namespace EY.DueDiligenceScreening.API.Screening.Infrastructure.Scrapers;


public class OfacScraperService : IOfacScraperService
{
    private readonly ILogger<OfacScraperService> _logger;
    private const string OFAC_URL = "https://sanctionssearch.ofac.treas.gov/";
    
    private const string NAME_INPUT_ID = "ctl00_MainContent_txtLastName";
    private const string SEARCH_BUTTON_ID = "ctl00_MainContent_btnSearch";
    private const string RESULTS_PANEL_ID = "ctl00_MainContent_pnlResults";
    private const string RESULTS_TABLE_ID = "gvSearchResults";
    private const string SCORE_SLIDER_ID = "ctl00_MainContent_Slider1_Boundcontrol";

    public OfacScraperService(ILogger<OfacScraperService> logger)
    {
        _logger = logger;
    }

    public async Task<List<OfacItem>> ScrapeAsync(GetInfoByCompanyNameQuery query)
    {
        var companyName = query.companyName;
        int minimumScore = 80;
        
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name cannot be empty", nameof(companyName));

        var results = new List<OfacItem>();

        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--headless"); 
        chromeOptions.AddArgument("--no-sandbox");
        chromeOptions.AddArgument("--disable-dev-shm-usage");
        chromeOptions.AddArgument("--disable-gpu");
        chromeOptions.AddArgument("--window-size=1920,1080");
        chromeOptions.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

        IWebDriver? driver = null;

        try
        {
            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);

            driver.Navigate().GoToUrl(OFAC_URL);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementExists(By.Id(NAME_INPUT_ID)));

            var jsExecutor = (IJavaScriptExecutor)driver;
            
            jsExecutor.ExecuteScript($@"
                try {{
                    let slider = $find('Slider1');
                    if (slider) {{
                        slider.set_Value({minimumScore});
                    }}
                }} catch(e) {{
                }}
                
                let input = document.getElementById('{SCORE_SLIDER_ID}');
                if (input) {{
                    input.value = {minimumScore};
                    
                    let changeEvent = new Event('change', {{ bubbles: true }});
                    input.dispatchEvent(changeEvent);
                    
                    let inputEvent = new Event('input', {{ bubbles: true }});
                    input.dispatchEvent(inputEvent);
                }}
            ");
            
            await Task.Delay(800);

            var nameInput = driver.FindElement(By.Id(NAME_INPUT_ID));
            nameInput.Clear();
            nameInput.SendKeys(companyName);


            var searchButton = driver.FindElement(By.Id(SEARCH_BUTTON_ID));
            searchButton.Click();

            await Task.Delay(2000); 

            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id(RESULTS_PANEL_ID)));
                
                var resultsTables = driver.FindElements(By.Id(RESULTS_TABLE_ID));
                
                if (resultsTables.Count == 0)
                {
                    return results;
                }

                var resultsTable = resultsTables[0];
                var rows = resultsTable.FindElements(By.TagName("tr"));

                foreach (var row in rows)
                {
                    try
                    {
                        var cells = row.FindElements(By.TagName("td"));
                        
                        if (cells.Count != 6)
                        {
                            continue;
                        }

                        var nameCell = cells[0];
                        var nameLink = nameCell.FindElements(By.TagName("a")).FirstOrDefault();
                        var name = nameLink?.Text.Trim() ?? nameCell.Text.Trim();
                        var detailsUrl = nameLink?.GetAttribute("href");

                        var address = cells[1].Text.Trim();
                        var type = cells[2].Text.Trim();
                        var programs = cells[3].Text.Trim();
                        var list = cells[4].Text.Trim();
                        var scoreText = cells[5].Text.Trim();
                        if (!int.TryParse(scoreText, out int score))
                        {
                            score = 0;
                        }
                        var item = new OfacItem(
                            name: name,
                            address: string.IsNullOrWhiteSpace(address) ? "N/A" : address,
                            type: type,
                            programs: programs,
                            list: list,
                            score: score,
                            detailsUrl: detailsUrl
                        );
                        results.Add(item);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }

            }
            catch (WebDriverTimeoutException)
            {
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to scrape OFAC for company: {companyName}", ex);
        }
        finally
        {
            driver?.Quit();
            driver?.Dispose();
        }

        return results;
    }
}

