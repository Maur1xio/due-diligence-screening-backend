namespace EY.DueDiligenceScreening.API.Providers.Domain.Model.Commands;

public record CreateProviderCommand(
    long UserId,
    string BusinessName,
    string TradeName,
    string TaxId,
    string PhoneNumber,
    string Email,
    string Website,
    string PhysicalAddress,
    string Country,
    decimal AnnualBillingUsd);

