using EY.DueDiligenceScreening.API.IAM.Domain.Model.Entities;

namespace EY.DueDiligenceScreening.API.IAM.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(long userID);
    Task<bool> ExistsByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}

