using EY.DueDiligenceScreening.API.IAM.Domain.Model.Commands;
using EY.DueDiligenceScreening.API.IAM.Domain.Model.Entities;

namespace EY.DueDiligenceScreening.API.IAM.Domain.Services;

public interface IAuthCommandService
{
    Task<(User User, string Token)> Handle(RegisterUserCommand command);
    Task<(User User, string Token)> Handle(SignInCommand command);
}

