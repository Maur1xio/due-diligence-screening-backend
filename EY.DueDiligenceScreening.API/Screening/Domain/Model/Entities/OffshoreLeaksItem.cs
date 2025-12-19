namespace EY.DueDiligenceScreening.API.Screening.Domain.Model.Entities;

public class OffshoreLeaksItem
{
    public string EntityName { get; set; } = string.Empty;
    public string EntityNodeUrl { get; set; } = string.Empty;
    public string Jurisdiction { get; set; } = string.Empty;
    public string LinkedTo { get; set; } = string.Empty;
    public string DataSource { get; set; } = string.Empty;
    public string DataSourceUrl { get; set; } = string.Empty;

    public OffshoreLeaksItem() { }

    public OffshoreLeaksItem(
        string entityName,
        string entityNodeUrl,
        string jurisdiction,
        string linkedTo,
        string dataSource,
        string dataSourceUrl)
    {
        EntityName = entityName;
        EntityNodeUrl = entityNodeUrl;
        Jurisdiction = jurisdiction;
        LinkedTo = linkedTo;
        DataSource = dataSource;
        DataSourceUrl = dataSourceUrl;
    }
}