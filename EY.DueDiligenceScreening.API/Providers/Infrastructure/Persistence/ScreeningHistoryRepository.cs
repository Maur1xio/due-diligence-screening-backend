using Microsoft.EntityFrameworkCore;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Providers.Domain.Repositories;
using EY.DueDiligenceScreening.API.Shared.Infrastructure.Persistence.EFC;

namespace EY.DueDiligenceScreening.API.Providers.Infrastructure.Persistence;

public class ScreeningHistoryRepository : IScreeningHistoryRepository
{
    private readonly AppDbContext _context;

    public ScreeningHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ScreeningHistory?> GetByProviderIdAsync(long providerId)
    {
        return await _context.ScreeningHistories
            .FirstOrDefaultAsync(h => h.ProviderId == providerId);
    }

    public async Task<ScreeningHistory?> GetByProviderIdWithResultsAsync(long providerId)
    {
        return await _context.ScreeningHistories
            .Include(h => h.OfacResults)
            .Include(h => h.OffshoreLeaksResults)
            .Include(h => h.WorldBankResults)
            .FirstOrDefaultAsync(h => h.ProviderId == providerId);
    }

    public async Task<ScreeningHistory> AddAsync(ScreeningHistory history)
    {
        await _context.ScreeningHistories.AddAsync(history);
        await _context.SaveChangesAsync();
        return history;
    }

    public async Task DeleteAsync(ScreeningHistory history)
    {
        _context.ScreeningHistories.Remove(history);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByProviderIdAsync(long providerId)
    {
        return await _context.ScreeningHistories
            .AnyAsync(h => h.ProviderId == providerId);
    }
}

