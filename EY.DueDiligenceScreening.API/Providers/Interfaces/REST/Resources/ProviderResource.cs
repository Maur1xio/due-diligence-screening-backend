namespace EY.DueDiligenceScreening.API.Providers.Interfaces.REST.Resources;

public record ProviderResource(
    long ProviderId,
    string BusinessName,
    string TradeName,
    string TaxId,
    string PhoneNumber,
    string Email,
    string Website,
    string PhysicalAddress,
    string Country,
    decimal AnnualBillingUsd,
    DateTime LastEditedAt,
    DateTime CreatedAt,
    bool HasScreeningHistory);

