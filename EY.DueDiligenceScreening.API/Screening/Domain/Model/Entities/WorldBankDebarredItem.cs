namespace EY.DueDiligenceScreening.API.Screening.Domain.Model.Entities;

public class WorldBankDebarredItem
{
    public string FirmName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string FromDate { get; set; } = string.Empty;
    public string ToDate { get; set; } = string.Empty;
    public string Grounds { get; set; } = string.Empty;

    public WorldBankDebarredItem() { }

    public WorldBankDebarredItem(
        string firmName,
        string address,
        string country,
        string fromDate,
        string toDate,
        string grounds)
    {
        FirmName = firmName;
        Address = address;
        Country = country;
        FromDate = fromDate;
        ToDate = toDate;
        Grounds = grounds;
    }
}

