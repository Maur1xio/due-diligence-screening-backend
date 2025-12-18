using EY.DueDiligenceScreening.API.IAM.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.IAM.Interfaces.REST.Resources;

namespace EY.DueDiligenceScreening.API.IAM.Interfaces.REST.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User user, string token, int expirationHours)
    {
        return new AuthenticatedUserResource(
            user.UserID,
            user.Email,
            user.FullName,
            token,
            DateTime.UtcNow.AddHours(expirationHours)
        );
    }
}

