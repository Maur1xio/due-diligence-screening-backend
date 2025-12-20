using EY.DueDiligenceScreening.API.Providers.Domain.Model.Commands;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;

namespace EY.DueDiligenceScreening.API.Providers.Domain.Services;

public interface IProviderCommandService
{
    Task<Provider> Handle(CreateProviderCommand command);
    Task<Provider> Handle(UpdateProviderCommand command);
    Task<bool> Handle(DeleteProviderCommand command);
}

