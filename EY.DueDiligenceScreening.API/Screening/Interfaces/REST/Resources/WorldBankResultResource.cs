namespace EY.DueDiligenceScreening.API.Screening.Interfaces.REST.Resources;

public record WorldBankResultResource(
    string CompanyName,
    int TotalResults,
    List<WorldBankItemResource> Items,
    DateTime ExecutedAt
);

public record WorldBankItemResource(
    string FirmName,
    string Address,
    string Country,
    string FromDate,
    string ToDate,
    string Grounds
);

