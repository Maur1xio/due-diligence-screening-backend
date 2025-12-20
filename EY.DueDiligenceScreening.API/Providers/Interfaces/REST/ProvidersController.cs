using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Commands;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Queries;
using EY.DueDiligenceScreening.API.Providers.Domain.Services;
using EY.DueDiligenceScreening.API.Providers.Interfaces.REST.Resources;
using EY.DueDiligenceScreening.API.Providers.Interfaces.REST.Transform;

namespace EY.DueDiligenceScreening.API.Providers.Interfaces.REST;

[ApiController]
[Route("api/v1/providers")]
[Authorize]
public class ProvidersController : ControllerBase
{
    private readonly IProviderCommandService _providerCommandService;
    private readonly IProviderQueryService _providerQueryService;
    private readonly IScreeningHistoryService _screeningHistoryService;

    public ProvidersController(
        IProviderCommandService providerCommandService,
        IProviderQueryService providerQueryService,
        IScreeningHistoryService screeningHistoryService)
    {
        _providerCommandService = providerCommandService;
        _providerQueryService = providerQueryService;
        _screeningHistoryService = screeningHistoryService;
    }

    private long GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? User.FindFirst("sub")?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("User ID not found in token");
        
        return userId;
    }

 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProviderResource>>> GetAllProviders()
    {
        var userId = GetCurrentUserId();
        var query = new GetProvidersByUserIdQuery(userId);
        var providers = await _providerQueryService.Handle(query);
        return Ok(ProviderResourceAssembler.ToResourceList(providers));
    }


    [HttpGet("{providerId:long}")]
    public async Task<ActionResult<ProviderResource>> GetProviderById(long providerId)
    {
        var userId = GetCurrentUserId();
        var query = new GetProviderByIdQuery(providerId, userId);
        var provider = await _providerQueryService.Handle(query);

        if (provider == null)
            return NotFound(new { message = $"Provider with id {providerId} not found" });

        return Ok(ProviderResourceAssembler.ToResource(provider));
    }


    [HttpPost]
    public async Task<ActionResult<ProviderResource>> CreateProvider([FromBody] CreateProviderRequest request)
    {
        var userId = GetCurrentUserId();
        
        var command = new CreateProviderCommand(
            userId,
            request.BusinessName,
            request.TradeName,
            request.TaxId,
            request.PhoneNumber,
            request.Email,
            request.Website ?? string.Empty,
            request.PhysicalAddress,
            request.Country,
            request.AnnualBillingUsd);

        var provider = await _providerCommandService.Handle(command);
        var resource = ProviderResourceAssembler.ToResource(provider);

        return CreatedAtAction(
            nameof(GetProviderById), 
            new { providerId = provider.ProviderId }, 
            resource);
    }

    [HttpPut("{providerId:long}")]
    public async Task<ActionResult<ProviderResource>> UpdateProvider(
        long providerId, 
        [FromBody] UpdateProviderRequest request)
    {
        var userId = GetCurrentUserId();

        var command = new UpdateProviderCommand(
            providerId,
            userId,
            request.BusinessName,
            request.TradeName,
            request.TaxId,
            request.PhoneNumber,
            request.Email,
            request.Website ?? string.Empty,
            request.PhysicalAddress,
            request.Country,
            request.AnnualBillingUsd);

        try
        {
            var provider = await _providerCommandService.Handle(command);
            return Ok(ProviderResourceAssembler.ToResource(provider));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Provider with id {providerId} not found" });
        }
    }

    [HttpDelete("{providerId:long}")]
    public async Task<IActionResult> DeleteProvider(long providerId)
    {
        var userId = GetCurrentUserId();
        var command = new DeleteProviderCommand(providerId, userId);
        var deleted = await _providerCommandService.Handle(command);

        if (!deleted)
            return NotFound(new { message = $"Provider with id {providerId} not found" });

        return NoContent();
    }

    [HttpGet("{providerId:long}/screening-history")]
    public async Task<ActionResult<ScreeningHistoryResource>> GetScreeningHistory(long providerId)
    {
        var userId = GetCurrentUserId();
        var query = new GetScreeningHistoryByProviderIdQuery(providerId, userId);
        var history = await _screeningHistoryService.Handle(query);

        if (history == null)
            return NotFound(new { message = "No screening history found for this provider" });

        return Ok(ScreeningHistoryResourceAssembler.ToResource(history));
    }

    [HttpPost("{providerId:long}/screening")]
    public async Task<ActionResult<ScreeningHistoryResource>> ExecuteScreening(
        long providerId,
        [FromBody] ExecuteScreeningRequest request)
    {
        var userId = GetCurrentUserId();

        var command = new ExecuteScreeningCommand(providerId, userId, request.Sources);

        try
        {
            var history = await _screeningHistoryService.Handle(command);
            return Ok(ScreeningHistoryResourceAssembler.ToResource(history));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Provider with id {providerId} not found" });
        }
    }
}

