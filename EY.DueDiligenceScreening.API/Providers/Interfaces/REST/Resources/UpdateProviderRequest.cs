using System.ComponentModel.DataAnnotations;

namespace EY.DueDiligenceScreening.API.Providers.Interfaces.REST.Resources;

public record UpdateProviderRequest(
    [Required][MaxLength(200)] string BusinessName,
    [Required][MaxLength(200)] string TradeName,
    [Required][StringLength(11, MinimumLength = 11)] string TaxId,
    [Required][Phone] string PhoneNumber,
    [Required][EmailAddress] string Email,
    [Url] string Website,
    [Required][MaxLength(500)] string PhysicalAddress,
    [Required][MaxLength(100)] string Country,
    [Required][Range(0, double.MaxValue)] decimal AnnualBillingUsd);

