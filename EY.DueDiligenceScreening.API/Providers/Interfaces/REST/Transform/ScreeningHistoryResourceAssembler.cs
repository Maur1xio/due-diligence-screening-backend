using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Providers.Interfaces.REST.Resources;

namespace EY.DueDiligenceScreening.API.Providers.Interfaces.REST.Transform;

public static class ScreeningHistoryResourceAssembler
{
    public static ScreeningHistoryResource ToResource(ScreeningHistory history)
    {
        return new ScreeningHistoryResource(
            history.ScreeningHistoryId,
            history.ProviderId,
            history.ScreenedAt,
            history.OfacResults.Select(r => new ProviderOfacResultResource(
                r.OfacResultId,
                r.Name,
                r.Address,
                r.Type,
                r.Programs,
                r.List,
                r.Score,
                r.DetailsUrl)).ToList(),
            history.OffshoreLeaksResults.Select(r => new ProviderOffshoreLeaksResultResource(
                r.OffshoreLeaksResultId,
                r.EntityName,
                r.EntityNodeUrl,
                r.Jurisdiction,
                r.LinkedTo,
                r.DataSource,
                r.DataSourceUrl)).ToList(),
            history.WorldBankResults.Select(r => new ProviderWorldBankResultResource(
                r.WorldBankResultId,
                r.FirmName,
                r.Address,
                r.Country,
                r.FromDate,
                r.ToDate,
                r.Grounds)).ToList());
    }
}

