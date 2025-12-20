using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Queries;
using EY.DueDiligenceScreening.API.Screening.Domain.Services;
using EY.DueDiligenceScreening.API.Screening.Interfaces.REST.Resources;
using EY.DueDiligenceScreening.API.Shared.Interfaces.Response;

namespace EY.DueDiligenceScreening.API.Screening.Interfaces.REST;

[ApiController]
[Route("api/v1/screening")]
[Authorize] 
public class ScreeningController : ControllerBase
{
    private readonly IOfacScraperService _ofacScraperService;
    private readonly IOffshoreLeaksScraperService _offshoreLeaksScraperService;
    private readonly IWorldBankScraperService _worldBankScraperService;
    private readonly IMultiSourceScreeningService _multiSourceScreeningService;

    public ScreeningController(
        IOfacScraperService ofacScraperService,
        IOffshoreLeaksScraperService offshoreLeaksScraperService,
        IWorldBankScraperService worldBankScraperService,
        IMultiSourceScreeningService multiSourceScreeningService,
        ILogger<ScreeningController> logger)
    {
        _ofacScraperService = ofacScraperService;
        _offshoreLeaksScraperService = offshoreLeaksScraperService;
        _worldBankScraperService = worldBankScraperService;
        _multiSourceScreeningService = multiSourceScreeningService;
    }

    [HttpPost("ofac")]
    [ProducesResponseType(typeof(OfacScrapeResultResource), 200)]
    [ProducesResponseType(typeof(ApiErrorResponse), 400)]
    [ProducesResponseType(typeof(ApiErrorResponse), 401)]
    [ProducesResponseType(typeof(ApiErrorResponse), 500)]
    public async Task<ActionResult<OfacScrapeResultResource>> ScrapeOfac(
        [FromBody] OfacScrapeRequestResource request,
        CancellationToken cancellationToken)
    {
        var query = new GetInfoByCompanyNameQuery(request.CompanyName);
        var results = await _ofacScraperService.ScrapeAsync(query, cancellationToken);

        var response = new OfacScrapeResultResource(
            CompanyName: request.CompanyName,
            TotalResults: results.Count,
            Items: results.Select(item => new OfacItemResource(
                Name: item.Name,
                Address: item.Address,
                Type: item.Type,
                Programs: item.Programs,
                List: item.List,
                Score: item.Score,
                DetailsUrl: item.DetailsUrl
            )).ToList(),
            ExecutedAt: DateTime.UtcNow
        );

        return Ok(response);
    }


    [HttpPost("offshore-leaks")]
    [ProducesResponseType(typeof(OffshoreLeaksResultResource), 200)]
    [ProducesResponseType(typeof(ApiErrorResponse), 400)]
    public async Task<ActionResult<OffshoreLeaksResultResource>> ScrapeOffshoreLeaks(
        [FromBody] OfacScrapeRequestResource request,
        CancellationToken cancellationToken) 
    {
        var query = new GetInfoByCompanyNameQuery(request.CompanyName);
        var results = await _offshoreLeaksScraperService.ScrapeAsync(query, cancellationToken);

        var response = new OffshoreLeaksResultResource(
            CompanyName: request.CompanyName,
            TotalResults: results.Count,
            Items: results.Select(item => new OffshoreLeaksItemResource(
                EntityName: item.EntityName,
                EntityNodeUrl: item.EntityNodeUrl,
                Jurisdiction: item.Jurisdiction,
                LinkedTo: item.LinkedTo,
                DataSource: item.DataSource,
                DataSourceUrl: item.DataSourceUrl
            )).ToList(),
            ExecutedAt: DateTime.UtcNow
        );

        return Ok(response);
    }

    [HttpPost("world-bank")]
    [ProducesResponseType(typeof(WorldBankResultResource), 200)]
    [ProducesResponseType(typeof(ApiErrorResponse), 400)]
    [ProducesResponseType(typeof(ApiErrorResponse), 401)]
    [ProducesResponseType(typeof(ApiErrorResponse), 500)]
    public async Task<ActionResult<WorldBankResultResource>> ScrapeWorldBank(
        [FromBody] OfacScrapeRequestResource request,
        CancellationToken cancellationToken)
    {
        var query = new GetInfoByCompanyNameQuery(request.CompanyName);
        var results = await _worldBankScraperService.ScrapeAsync(query, cancellationToken);

        var response = new WorldBankResultResource(
            CompanyName: request.CompanyName,
            TotalResults: results.Count,
            Items: results.Select(item => new WorldBankItemResource(
                FirmName: item.FirmName,
                Address: item.Address,
                Country: item.Country,
                FromDate: item.FromDate,
                ToDate: item.ToDate,
                Grounds: item.Grounds
            )).ToList(),
            ExecutedAt: DateTime.UtcNow
        );

        return Ok(response);
    }

    [HttpPost("multi-source")]
    [ProducesResponseType(typeof(MultiSourceScreeningResponse), 200)]
    [ProducesResponseType(typeof(ApiErrorResponse), 400)]
    [ProducesResponseType(typeof(ApiErrorResponse), 401)]
    [ProducesResponseType(typeof(ApiErrorResponse), 500)]
    public async Task<ActionResult<MultiSourceScreeningResponse>> ExecuteMultiSourceScreening(
        [FromBody] MultiSourceScreeningRequest request,
        CancellationToken cancellationToken)
    {
        var query = new MultiSourceScreeningQuery(
            request.CompanyName,
            request.Sources
        );

        var result = await _multiSourceScreeningService.ExecuteScreeningAsync(query, cancellationToken);

        var response = new MultiSourceScreeningResponse(
            CompanyName: result.CompanyName,
            TotalSources: result.TotalSources,
            SuccessfulSources: result.SuccessfulSources,
            FailedSources: result.FailedSources,
            TotalMatches: result.TotalMatches,
            SourceResults: result.SourceResults.Select(sr => new SourceResultResource(
                Source: sr.Source.ToString(),
                TotalResults: sr.TotalResults,
                Success: sr.Success,
                ErrorMessage: sr.ErrorMessage,
                Data: sr.Data
            )).ToList()
        );

        return Ok(response);
    }
}

