using EY.DueDiligenceScreening.API.Screening.Domain.Model.ValueObjects;

namespace EY.DueDiligenceScreening.API.Screening.Domain.Model.Queries;

public record MultiSourceScreeningQuery(
    string CompanyName,
    List<ScreeningSource> Sources
);

