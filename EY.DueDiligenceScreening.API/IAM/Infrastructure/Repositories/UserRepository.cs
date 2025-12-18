using Microsoft.EntityFrameworkCore;
using EY.DueDiligenceScreening.API.IAM.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.IAM.Domain.Repositories;
using EY.DueDiligenceScreening.API.Shared.Infrastructure.Persistence.EFC;

namespace EY.DueDiligenceScreening.API.IAM.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> FindByIdAsync(long userID)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserID == userID);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}

