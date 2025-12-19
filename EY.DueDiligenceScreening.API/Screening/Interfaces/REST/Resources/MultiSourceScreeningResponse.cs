namespace EY.DueDiligenceScreening.API.Screening.Interfaces.REST.Resources;

/// <summary>
/// Response para screening multi-fuente
/// </summary>
public record MultiSourceScreeningResponse(
    string CompanyName,
    int TotalSources,
    int SuccessfulSources,
    int FailedSources,
    int TotalMatches,
    List<SourceResultResource> SourceResults
);

/// <summary>
/// Resultado de una fuente específica
/// </summary>
public record SourceResultResource(
    string Source,
    int TotalResults,
    bool Success,
    string? ErrorMessage,
    object Data
);

