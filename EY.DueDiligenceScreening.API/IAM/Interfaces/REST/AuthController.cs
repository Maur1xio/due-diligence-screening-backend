using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using EY.DueDiligenceScreening.API.IAM.Domain.Services;
using EY.DueDiligenceScreening.API.IAM.Interfaces.REST.Resources;
using EY.DueDiligenceScreening.API.IAM.Interfaces.REST.Transform;
using EY.DueDiligenceScreening.API.Shared.Infrastructure.Settings;
using EY.DueDiligenceScreening.API.Shared.Interfaces.Response;
using EY.DueDiligenceScreening.API.IAM.Domain.Model.Queries;

namespace EY.DueDiligenceScreening.API.IAM.Interfaces.REST;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthCommandService _authCommandService;
    private readonly IAuthQueryService _authQueryService;
    private readonly JwtSettings _jwtSettings;

    public AuthController(
        IAuthCommandService authCommandService,
        IAuthQueryService authQueryService,
        IOptions<JwtSettings> jwtSettings)
    {
        _authCommandService = authCommandService;
        _authQueryService = authQueryService;
        _jwtSettings = jwtSettings.Value;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticatedUserResource), 201)]
    [ProducesResponseType(typeof(ApiErrorResponse), 400)]
    public async Task<ActionResult<AuthenticatedUserResource>> Register([FromBody] RegisterUserResource resource)
    {
        var command = RegisterUserCommandFromResourceAssembler.ToCommandFromResource(resource);
        var (user, token) = await _authCommandService.Handle(command);

        var response = AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(
            user,
            token,
            _jwtSettings.ExpirationHours
        );

        return CreatedAtAction(nameof(Register), new { id = user.UserID }, response);
    }

    /// <summary>
    /// Sign in and get JWT token
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticatedUserResource), 200)]
    [ProducesResponseType(typeof(ApiErrorResponse), 401)]
    public async Task<ActionResult<AuthenticatedUserResource>> Login([FromBody] SignInResource resource)
    {
        var command = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
        var (user, token) = await _authCommandService.Handle(command);

        var response = AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(
            user,
            token,
            _jwtSettings.ExpirationHours
        );

        return Ok(response);
    }

    /// <summary>
    /// Get current authenticated user info
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(ApiErrorResponse), 404)]
    public async Task<ActionResult<object>> GetCurrentUser()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => 
            c.Type == System.Security.Claims.ClaimTypes.NameIdentifier || 
            c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User ID not found in token");

        var userId = long.Parse(userIdClaim);
        var query = new EY.DueDiligenceScreening.API.IAM.Domain.Model.Queries.GetUserByIdQuery(userId);
        var user = await _authQueryService.Handle(query);

        if (user == null)
            throw new KeyNotFoundException("User not found in database");

        return Ok(new
        {
            userId = user.UserID,
            email = user.Email,
            fullName = user.FullName
        });
    }
}

