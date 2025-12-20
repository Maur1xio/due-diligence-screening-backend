using System.Diagnostics;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Queries;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.ValueObjects;
using EY.DueDiligenceScreening.API.Screening.Domain.Services;

namespace EY.DueDiligenceScreening.API.Screening.Application.Services;

public class MultiSourceScreeningService : IMultiSourceScreeningService
{
    private readonly IOfacScraperService _ofacScraperService;
    private readonly IOffshoreLeaksScraperService _offshoreLeaksScraperService;
    private readonly IWorldBankScraperService _worldBankScraperService;
    private readonly ILogger<MultiSourceScreeningService> _logger;

    public MultiSourceScreeningService(
        IOfacScraperService ofacScraperService,
        IOffshoreLeaksScraperService offshoreLeaksScraperService,
        IWorldBankScraperService worldBankScraperService,
        ILogger<MultiSourceScreeningService> logger)
    {
        _ofacScraperService = ofacScraperService;
        _offshoreLeaksScraperService = offshoreLeaksScraperService;
        _worldBankScraperService = worldBankScraperService;
        _logger = logger;
    }

    public async Task<ScreeningResult> ExecuteScreeningAsync(MultiSourceScreeningQuery query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.CompanyName))
            throw new ArgumentException("Company name cannot be empty", nameof(query.CompanyName));

        if (query.Sources == null || !query.Sources.Any())
            throw new ArgumentException("At least one source must be specified", nameof(query.Sources));

        var overallStopwatch = Stopwatch.StartNew();
        var companyQuery = new GetInfoByCompanyNameQuery(query.CompanyName);

        var tasks = new List<Task<SourceResult>>();

        foreach (var source in query.Sources)
        {
            tasks.Add(ExecuteSourceScreeningAsync(source, companyQuery, cancellationToken));
        }

        try
        {
            var sourceResults = await Task.WhenAll(tasks);
            overallStopwatch.Stop();

            var result = new ScreeningResult(
                query.CompanyName,
                sourceResults.ToList(),
                DateTime.UtcNow,
                overallStopwatch.Elapsed
            );

            return result;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("⚠️ Multi-source screening cancelled for company: {CompanyName}", query.CompanyName);
            throw; 
        }
    }

    private async Task<SourceResult> ExecuteSourceScreeningAsync(
        ScreeningSource source, 
        GetInfoByCompanyNameQuery query,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            object data;
            int totalResults;

            switch (source)
            {
                case ScreeningSource.OFAC:
                    var ofacResults = await _ofacScraperService.ScrapeAsync(query, cancellationToken);
                    data = ofacResults;
                    totalResults = ofacResults.Count;
                    break;

                case ScreeningSource.OffshoreLeaks:
                    var offshoreResults = await _offshoreLeaksScraperService.ScrapeAsync(query, cancellationToken);
                    data = offshoreResults;
                    totalResults = offshoreResults.Count;
                    break;

                case ScreeningSource.WorldBank:
                    var worldBankResults = await _worldBankScraperService.ScrapeAsync(query, cancellationToken);
                    data = worldBankResults;
                    totalResults = worldBankResults.Count;
                    break;

                default:
                    throw new ArgumentException($"Unknown source: {source}");
            }

            stopwatch.Stop();

            return new SourceResult(
                source,
                totalResults,
                success: true,
                data,
                stopwatch.Elapsed
            );
        }
        catch (OperationCanceledException)
        {
            stopwatch.Stop();
            _logger.LogWarning("⚠️ Scraping cancelled for source: {Source}", source);
            
            return new SourceResult(
                source,
                totalResults: 0,
                success: false,
                data: new { },
                stopwatch.Elapsed,
                errorMessage: "Operation was cancelled by client"
            );
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "❌ Error scraping source: {Source}", source);

            return new SourceResult(
                source,
                totalResults: 0,
                success: false,
                data: new { },
                stopwatch.Elapsed,
                errorMessage: ex.Message
            );
        }
    }
}