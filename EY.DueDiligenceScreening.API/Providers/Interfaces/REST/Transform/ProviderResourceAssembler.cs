using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Providers.Interfaces.REST.Resources;

namespace EY.DueDiligenceScreening.API.Providers.Interfaces.REST.Transform;

public static class ProviderResourceAssembler
{
    public static ProviderResource ToResource(Provider provider)
    {
        return new ProviderResource(
            provider.ProviderId,
            provider.BusinessName,
            provider.TradeName,
            provider.TaxId,
            provider.PhoneNumber,
            provider.Email,
            provider.Website,
            provider.PhysicalAddress,
            provider.Country,
            provider.AnnualBillingUsd,
            provider.LastEditedAt,
            provider.CreatedAt,
            provider.ScreeningHistory != null);
    }

    public static IEnumerable<ProviderResource> ToResourceList(IEnumerable<Provider> providers)
    {
        return providers.Select(ToResource);
    }
}

