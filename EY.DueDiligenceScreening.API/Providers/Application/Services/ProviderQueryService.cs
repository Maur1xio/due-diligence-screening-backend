using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Queries;
using EY.DueDiligenceScreening.API.Providers.Domain.Repositories;
using EY.DueDiligenceScreening.API.Providers.Domain.Services;

namespace EY.DueDiligenceScreening.API.Providers.Application.Services;

public class ProviderQueryService : IProviderQueryService
{
    private readonly IProviderRepository _providerRepository;

    public ProviderQueryService(IProviderRepository providerRepository)
    {
        _providerRepository = providerRepository;
    }

    public async Task<Provider?> Handle(GetProviderByIdQuery query)
    {
        return await _providerRepository.GetByIdAndUserIdAsync(query.ProviderId, query.UserId);
    }

    public async Task<IEnumerable<Provider>> Handle(GetProvidersByUserIdQuery query)
    {
        return await _providerRepository.GetByUserIdAsync(query.UserId);
    }
}

