namespace EY.DueDiligenceScreening.API.IAM.Domain.Model.Commands;

public record RegisterUserCommand(
    string Email,
    string Password,
    string FullName
);

