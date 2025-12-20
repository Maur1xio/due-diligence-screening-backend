using EY.DueDiligenceScreening.API.IAM.Domain.Model.Commands;
using EY.DueDiligenceScreening.API.IAM.Interfaces.REST.Resources;

namespace EY.DueDiligenceScreening.API.IAM.Interfaces.REST.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(
            resource.Email,
            resource.Password
        );
    }
}

