using EY.DueDiligenceScreening.API.Providers.Domain.Model.Commands;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Queries;

namespace EY.DueDiligenceScreening.API.Providers.Domain.Services;

public interface IScreeningHistoryService
{
    Task<ScreeningHistory?> Handle(GetScreeningHistoryByProviderIdQuery query);
    Task<ScreeningHistory> Handle(ExecuteScreeningCommand command);
}

