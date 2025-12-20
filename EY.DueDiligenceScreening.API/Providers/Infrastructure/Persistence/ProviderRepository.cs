using Microsoft.EntityFrameworkCore;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Providers.Domain.Repositories;
using EY.DueDiligenceScreening.API.Shared.Infrastructure.Persistence.EFC;

namespace EY.DueDiligenceScreening.API.Providers.Infrastructure.Persistence;

public class ProviderRepository : IProviderRepository
{
    private readonly AppDbContext _context;

    public ProviderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Provider?> GetByIdAsync(long providerId)
    {
        return await _context.Providers
            .FirstOrDefaultAsync(p => p.ProviderId == providerId);
    }

    public async Task<Provider?> GetByIdAndUserIdAsync(long providerId, long userId)
    {
        return await _context.Providers
            .FirstOrDefaultAsync(p => p.ProviderId == providerId && p.UserId == userId);
    }

    public async Task<IEnumerable<Provider>> GetByUserIdAsync(long userId)
    {
        return await _context.Providers
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Provider> AddAsync(Provider provider)
    {
        await _context.Providers.AddAsync(provider);
        await _context.SaveChangesAsync();
        return provider;
    }

    public async Task<Provider> UpdateAsync(Provider provider)
    {
        _context.Providers.Update(provider);
        await _context.SaveChangesAsync();
        return provider;
    }

    public async Task DeleteAsync(Provider provider)
    {
        _context.Providers.Remove(provider);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(long providerId, long userId)
    {
        return await _context.Providers
            .AnyAsync(p => p.ProviderId == providerId && p.UserId == userId);
    }
}

