namespace EY.DueDiligenceScreening.API.Screening.Domain.Services;

using EY.DueDiligenceScreening.API.Screening.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Queries;

public interface IWorldBankScraperService
{
    Task<List<WorldBankDebarredItem>> ScrapeAsync(GetInfoByCompanyNameQuery query, CancellationToken cancellationToken = default);
}

