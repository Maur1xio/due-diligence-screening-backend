namespace EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;

public class OffshoreLeaksScreeningResult
{
    public long OffshoreLeaksResultId { get; set; }
    public long ScreeningHistoryId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string EntityNodeUrl { get; set; } = string.Empty;
    public string Jurisdiction { get; set; } = string.Empty;
    public string LinkedTo { get; set; } = string.Empty;
    public string DataSource { get; set; } = string.Empty;
    public string DataSourceUrl { get; set; } = string.Empty;

    public ScreeningHistory ScreeningHistory { get; set; } = null!;

    public OffshoreLeaksScreeningResult() { }

    public OffshoreLeaksScreeningResult(
        long screeningHistoryId,
        string entityName,
        string entityNodeUrl,
        string jurisdiction,
        string linkedTo,
        string dataSource,
        string dataSourceUrl)
    {
        ScreeningHistoryId = screeningHistoryId;
        EntityName = entityName;
        EntityNodeUrl = entityNodeUrl;
        Jurisdiction = jurisdiction;
        LinkedTo = linkedTo;
        DataSource = dataSource;
        DataSourceUrl = dataSourceUrl;
    }
}

