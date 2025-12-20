namespace EY.DueDiligenceScreening.API.Screening.Interfaces.REST.Resources;

public record OfacScrapeResultResource(
    string CompanyName,
    int TotalResults,
    List<OfacItemResource> Items,
    DateTime ExecutedAt
);

public record OfacItemResource(
    string Name,
    string Address,
    string Type,
    string Programs,
    string List,
    int Score,
    string? DetailsUrl
);

