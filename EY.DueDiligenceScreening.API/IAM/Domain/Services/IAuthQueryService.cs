using EY.DueDiligenceScreening.API.IAM.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.IAM.Domain.Model.Queries;

namespace EY.DueDiligenceScreening.API.IAM.Domain.Services;

public interface IAuthQueryService
{
    Task<User?> Handle(GetUserByIdQuery query);
    Task<User?> Handle(GetUserByEmailQuery query);
}

