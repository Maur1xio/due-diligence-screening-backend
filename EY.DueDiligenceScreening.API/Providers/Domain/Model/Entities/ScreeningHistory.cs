namespace EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;

public class ScreeningHistory
{
    public long ScreeningHistoryId { get; set; }
    public long ProviderId { get; set; }
    public DateTime ScreenedAt { get; set; }

    public Provider Provider { get; set; } = null!;
    public List<OfacScreeningResult> OfacResults { get; set; } = new();
    public List<OffshoreLeaksScreeningResult> OffshoreLeaksResults { get; set; } = new();
    public List<WorldBankScreeningResult> WorldBankResults { get; set; } = new();

    public ScreeningHistory() { }

    public ScreeningHistory(long providerId)
    {
        ProviderId = providerId;
        ScreenedAt = DateTime.UtcNow;
    }
}

