using EY.DueDiligenceScreening.API.IAM.Domain.Model.Entities;

namespace EY.DueDiligenceScreening.API.IAM.Domain.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    long? ValidateToken(string token);
}

