using EY.DueDiligenceScreening.API.Providers.Domain.Model.Commands;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Queries;
using EY.DueDiligenceScreening.API.Providers.Domain.Repositories;
using EY.DueDiligenceScreening.API.Providers.Domain.Services;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.Queries;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.ValueObjects;
using EY.DueDiligenceScreening.API.Screening.Domain.Services;

namespace EY.DueDiligenceScreening.API.Providers.Application.Services;

public class ScreeningHistoryService : IScreeningHistoryService
{
    private readonly IScreeningHistoryRepository _historyRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly IMultiSourceScreeningService _multiSourceScreeningService;

    public ScreeningHistoryService(
        IScreeningHistoryRepository historyRepository,
        IProviderRepository providerRepository,
        IMultiSourceScreeningService multiSourceScreeningService)
    {
        _historyRepository = historyRepository;
        _providerRepository = providerRepository;
        _multiSourceScreeningService = multiSourceScreeningService;
    }

    public async Task<ScreeningHistory?> Handle(GetScreeningHistoryByProviderIdQuery query)
    {
        var providerExists = await _providerRepository.ExistsAsync(query.ProviderId, query.UserId);
        if (!providerExists)
            return null;

        return await _historyRepository.GetByProviderIdWithResultsAsync(query.ProviderId);
    }

    public async Task<ScreeningHistory> Handle(ExecuteScreeningCommand command)
    {
        var provider = await _providerRepository.GetByIdAndUserIdAsync(command.ProviderId, command.UserId)
            ?? throw new KeyNotFoundException($"Provider with id {command.ProviderId} not found");

        var screeningQuery = new MultiSourceScreeningQuery(provider.TradeName, command.Sources);
        var screeningResult = await _multiSourceScreeningService.ExecuteScreeningAsync(screeningQuery);

        var existingHistory = await _historyRepository.GetByProviderIdAsync(command.ProviderId);
        if (existingHistory != null)
        {
            await _historyRepository.DeleteAsync(existingHistory);
        }

        var newHistory = new ScreeningHistory(command.ProviderId);

        foreach (var sourceResult in screeningResult.SourceResults)
        {
            if (!sourceResult.Success)
                continue;

            switch (sourceResult.Source)
            {
                case ScreeningSource.OFAC:
                    MapOfacResults(newHistory, sourceResult.Data);
                    break;
                case ScreeningSource.OffshoreLeaks:
                    MapOffshoreLeaksResults(newHistory, sourceResult.Data);
                    break;
                case ScreeningSource.WorldBank:
                    MapWorldBankResults(newHistory, sourceResult.Data);
                    break;
            }
        }

        return await _historyRepository.AddAsync(newHistory);
    }

    private static void MapOfacResults(ScreeningHistory history, object data)
    {
        if (data is not List<Screening.Domain.Model.Entities.OfacItem> items)
            return;

        foreach (var item in items)
        {
            history.OfacResults.Add(new OfacScreeningResult(
                history.ScreeningHistoryId,
                item.Name,
                item.Address,
                item.Type,
                item.Programs,
                item.List,
                item.Score,
                item.DetailsUrl));
        }
    }

    private static void MapOffshoreLeaksResults(ScreeningHistory history, object data)
    {
        if (data is not List<Screening.Domain.Model.Entities.OffshoreLeaksItem> items)
            return;

        foreach (var item in items)
        {
            history.OffshoreLeaksResults.Add(new OffshoreLeaksScreeningResult(
                history.ScreeningHistoryId,
                item.EntityName,
                item.EntityNodeUrl,
                item.Jurisdiction,
                item.LinkedTo,
                item.DataSource,
                item.DataSourceUrl));
        }
    }

    private static void MapWorldBankResults(ScreeningHistory history, object data)
    {
        if (data is not List<Screening.Domain.Model.Entities.WorldBankDebarredItem> items)
            return;

        foreach (var item in items)
        {
            history.WorldBankResults.Add(new WorldBankScreeningResult(
                history.ScreeningHistoryId,
                item.FirmName,
                item.Address,
                item.Country,
                item.FromDate,
                item.ToDate,
                item.Grounds));
        }
    }
}

