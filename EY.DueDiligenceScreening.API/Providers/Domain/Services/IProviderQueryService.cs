using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Queries;

namespace EY.DueDiligenceScreening.API.Providers.Domain.Services;

public interface IProviderQueryService
{
    Task<Provider?> Handle(GetProviderByIdQuery query);
    Task<IEnumerable<Provider>> Handle(GetProvidersByUserIdQuery query);
}

