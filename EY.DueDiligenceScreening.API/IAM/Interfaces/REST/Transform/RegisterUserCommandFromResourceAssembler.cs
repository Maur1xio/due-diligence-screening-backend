using EY.DueDiligenceScreening.API.IAM.Domain.Model.Commands;
using EY.DueDiligenceScreening.API.IAM.Interfaces.REST.Resources;

namespace EY.DueDiligenceScreening.API.IAM.Interfaces.REST.Transform;

public static class RegisterUserCommandFromResourceAssembler
{
    public static RegisterUserCommand ToCommandFromResource(RegisterUserResource resource)
    {
        return new RegisterUserCommand(
            resource.Email,
            resource.Password,
            resource.FullName
        );
    }
}

