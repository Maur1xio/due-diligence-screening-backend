using EY.DueDiligenceScreening.API.Screening.Domain.Model.ValueObjects;

namespace EY.DueDiligenceScreening.API.Providers.Domain.Model.Commands;

public record ExecuteScreeningCommand(
    long ProviderId,
    long UserId,
    List<ScreeningSource> Sources);

