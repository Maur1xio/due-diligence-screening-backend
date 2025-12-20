namespace EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;

public class WorldBankScreeningResult
{
    public long WorldBankResultId { get; set; }
    public long ScreeningHistoryId { get; set; }
    public string FirmName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string FromDate { get; set; } = string.Empty;
    public string ToDate { get; set; } = string.Empty;
    public string Grounds { get; set; } = string.Empty;

    public ScreeningHistory ScreeningHistory { get; set; } = null!;

    public WorldBankScreeningResult() { }

    public WorldBankScreeningResult(
        long screeningHistoryId,
        string firmName,
        string address,
        string country,
        string fromDate,
        string toDate,
        string grounds)
    {
        ScreeningHistoryId = screeningHistoryId;
        FirmName = firmName;
        Address = address;
        Country = country;
        FromDate = fromDate;
        ToDate = toDate;
        Grounds = grounds;
    }
}

