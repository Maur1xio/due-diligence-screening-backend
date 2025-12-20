namespace EY.DueDiligenceScreening.API.Providers.Interfaces.REST.Resources;

public record ScreeningHistoryResource(
    long ScreeningHistoryId,
    long ProviderId,
    DateTime ScreenedAt,
    List<ProviderOfacResultResource> OfacResults,
    List<ProviderOffshoreLeaksResultResource> OffshoreLeaksResults,
    List<ProviderWorldBankResultResource> WorldBankResults);

public record ProviderOfacResultResource(
    long Id,
    string Name,
    string Address,
    string Type,
    string Programs,
    string List,
    int Score,
    string? DetailsUrl);

public record ProviderOffshoreLeaksResultResource(
    long Id,
    string EntityName,
    string EntityNodeUrl,
    string Jurisdiction,
    string LinkedTo,
    string DataSource,
    string DataSourceUrl);

public record ProviderWorldBankResultResource(
    long Id,
    string FirmName,
    string Address,
    string Country,
    string FromDate,
    string ToDate,
    string Grounds);

