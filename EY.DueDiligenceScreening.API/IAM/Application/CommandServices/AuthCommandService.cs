using EY.DueDiligenceScreening.API.IAM.Domain.Model.Commands;
using EY.DueDiligenceScreening.API.IAM.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.IAM.Domain.Repositories;
using EY.DueDiligenceScreening.API.IAM.Domain.Services;

namespace EY.DueDiligenceScreening.API.IAM.Application.CommandServices;

public class AuthCommandService : IAuthCommandService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthCommandService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<(User User, string Token)> Handle(RegisterUserCommand command)
    {
        if (!IsValidEmail(command.Email))
            throw new ArgumentException("Invalid email format");

        if (await _userRepository.ExistsByEmailAsync(command.Email))
            throw new InvalidOperationException("User with this email already exists");

        if (command.Password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);

        var user = new User(command.Email, passwordHash, command.FullName);

        await _userRepository.AddAsync(user);

        var token = _tokenService.GenerateToken(user);

        return (user, token);
    }

    public async Task<(User User, string Token)> Handle(SignInCommand command)
    {
        var user = await _userRepository.FindByEmailAsync(command.Email);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password");

        if (!BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");

        var token = _tokenService.GenerateToken(user);

        return (user, token);
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

