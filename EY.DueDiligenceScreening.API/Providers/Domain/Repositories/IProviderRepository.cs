using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;

namespace EY.DueDiligenceScreening.API.Providers.Domain.Repositories;

public interface IProviderRepository
{
    Task<Provider?> GetByIdAsync(long providerId);
    Task<Provider?> GetByIdAndUserIdAsync(long providerId, long userId);
    Task<IEnumerable<Provider>> GetByUserIdAsync(long userId);
    Task<Provider> AddAsync(Provider provider);
    Task<Provider> UpdateAsync(Provider provider);
    Task DeleteAsync(Provider provider);
    Task<bool> ExistsAsync(long providerId, long userId);
}

