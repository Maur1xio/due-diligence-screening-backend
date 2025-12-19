using EY.DueDiligenceScreening.API.Screening.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Queries;

namespace EY.DueDiligenceScreening.API.Screening.Domain.Services;


public interface IOfacScraperService
{
    Task<List<OfacItem>> ScrapeAsync(GetInfoByCompanyNameQuery query);
}

