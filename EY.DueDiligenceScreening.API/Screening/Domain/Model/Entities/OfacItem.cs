namespace EY.DueDiligenceScreening.API.Screening.Domain.Model.Entities;


public class OfacItem
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Programs { get; set; } = string.Empty;
    public string List { get; set; } = string.Empty;
    public int Score { get; set; }
    public string? DetailsUrl { get; set; }

    public OfacItem()
    {
    }

    public OfacItem(
        string name,
        string address,
        string type,
        string programs,
        string list,
        int score,
        string? detailsUrl = null)
    {
        Name = name;
        Address = address;
        Type = type;
        Programs = programs;
        List = list;
        Score = score;
        DetailsUrl = detailsUrl;
    }
}

