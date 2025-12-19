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

    public ScreeningController(
        IOfacScraperService ofacScraperService,
        ILogger<ScreeningController> logger)
    {
        _ofacScraperService = ofacScraperService;
    }

    [HttpPost("ofac")]
    [ProducesResponseType(typeof(OfacScrapeResultResource), 200)]
    [ProducesResponseType(typeof(ApiErrorResponse), 400)]
    [ProducesResponseType(typeof(ApiErrorResponse), 401)]
    [ProducesResponseType(typeof(ApiErrorResponse), 500)]
    public async Task<ActionResult<OfacScrapeResultResource>> ScrapeOfac(
        [FromBody] OfacScrapeRequestResource request)
    {
        var query = new GetInfoByCompanyNameQuery(request.CompanyName);
        var results = await _ofacScraperService.ScrapeAsync(query);

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
}

