using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;

namespace EY.DueDiligenceScreening.API.Providers.Domain.Repositories;

public interface IScreeningHistoryRepository
{
    Task<ScreeningHistory?> GetByProviderIdAsync(long providerId);
    Task<ScreeningHistory?> GetByProviderIdWithResultsAsync(long providerId);
    Task<ScreeningHistory> AddAsync(ScreeningHistory history);
    Task DeleteAsync(ScreeningHistory history);
    Task<bool> ExistsByProviderIdAsync(long providerId);
}

