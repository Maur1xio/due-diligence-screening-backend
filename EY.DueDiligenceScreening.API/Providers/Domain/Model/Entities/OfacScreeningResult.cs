namespace EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;

public class OfacScreeningResult
{
    public long OfacResultId { get; set; }
    public long ScreeningHistoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Programs { get; set; } = string.Empty;
    public string List { get; set; } = string.Empty;
    public int Score { get; set; }
    public string? DetailsUrl { get; set; }

    public ScreeningHistory ScreeningHistory { get; set; } = null!;

    public OfacScreeningResult() { }

    public OfacScreeningResult(
        long screeningHistoryId,
        string name,
        string address,
        string type,
        string programs,
        string list,
        int score,
        string? detailsUrl)
    {
        ScreeningHistoryId = screeningHistoryId;
        Name = name;
        Address = address;
        Type = type;
        Programs = programs;
        List = list;
        Score = score;
        DetailsUrl = detailsUrl;
    }
}

