using EY.DueDiligenceScreening.API.IAM.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.IAM.Domain.Model.Queries;
using EY.DueDiligenceScreening.API.IAM.Domain.Repositories;
using EY.DueDiligenceScreening.API.IAM.Domain.Services;

namespace EY.DueDiligenceScreening.API.IAM.Application.QueryServices;

public class AuthQueryService : IAuthQueryService
{
    private readonly IUserRepository _userRepository;

    public AuthQueryService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> Handle(GetUserByIdQuery query)
    {
        return await _userRepository.FindByIdAsync(query.UserID);
    }

    public async Task<User?> Handle(GetUserByEmailQuery query)
    {
        return await _userRepository.FindByEmailAsync(query.Email);
    }
}

