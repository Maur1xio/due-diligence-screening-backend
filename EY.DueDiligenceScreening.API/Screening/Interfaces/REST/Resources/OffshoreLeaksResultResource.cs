namespace EY.DueDiligenceScreening.API.Screening.Interfaces.REST.Resources;

public record OffshoreLeaksResultResource(
    string CompanyName,
    int TotalResults,
    List<OffshoreLeaksItemResource> Items,
    DateTime ExecutedAt
);

public record OffshoreLeaksItemResource(
    string EntityName,
    string EntityNodeUrl,
    string Jurisdiction,
    string LinkedTo,
    string DataSource,
    string DataSourceUrl
);

