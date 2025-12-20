using EY.DueDiligenceScreening.API.Providers.Domain.Model.Commands;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Providers.Domain.Repositories;
using EY.DueDiligenceScreening.API.Providers.Domain.Services;

namespace EY.DueDiligenceScreening.API.Providers.Application.Services;

public class ProviderCommandService : IProviderCommandService
{
    private readonly IProviderRepository _providerRepository;

    public ProviderCommandService(IProviderRepository providerRepository)
    {
        _providerRepository = providerRepository;
    }

    public async Task<Provider> Handle(CreateProviderCommand command)
    {
        var provider = new Provider(
            command.UserId,
            command.BusinessName,
            command.TradeName,
            command.TaxId,
            command.PhoneNumber,
            command.Email,
            command.Website,
            command.PhysicalAddress,
            command.Country,
            command.AnnualBillingUsd);

        return await _providerRepository.AddAsync(provider);
    }

    public async Task<Provider> Handle(UpdateProviderCommand command)
    {
        var provider = await _providerRepository.GetByIdAndUserIdAsync(command.ProviderId, command.UserId)
            ?? throw new KeyNotFoundException($"Provider with id {command.ProviderId} not found");

        provider.Update(
            command.BusinessName,
            command.TradeName,
            command.TaxId,
            command.PhoneNumber,
            command.Email,
            command.Website,
            command.PhysicalAddress,
            command.Country,
            command.AnnualBillingUsd);

        return await _providerRepository.UpdateAsync(provider);
    }

    public async Task<bool> Handle(DeleteProviderCommand command)
    {
        var provider = await _providerRepository.GetByIdAndUserIdAsync(command.ProviderId, command.UserId);
        
        if (provider == null)
            return false;

        await _providerRepository.DeleteAsync(provider);
        return true;
    }
}

