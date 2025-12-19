namespace EY.DueDiligenceScreening.API.Screening.Domain.Model.Entities;

/// <summary>
/// Resultado agregado de screening en múltiples fuentes
/// </summary>
public class ScreeningResult
{
    public string CompanyName { get; set; } = string.Empty;
    public int TotalSources { get; set; }
    public int SuccessfulSources { get; set; }
    public int FailedSources { get; set; }
    public int TotalMatches { get; set; }
    public List<SourceResult> SourceResults { get; set; } = new();
    public DateTime ExecutedAt { get; set; }
    public TimeSpan TotalExecutionTime { get; set; }

    public ScreeningResult() { }

    public ScreeningResult(
        string companyName,
        List<SourceResult> sourceResults,
        DateTime executedAt,
        TimeSpan totalExecutionTime)
    {
        CompanyName = companyName;
        SourceResults = sourceResults;
        ExecutedAt = executedAt;
        TotalExecutionTime = totalExecutionTime;
        
        // Calcular métricas agregadas
        TotalSources = sourceResults.Count;
        SuccessfulSources = sourceResults.Count(s => s.Success);
        FailedSources = sourceResults.Count(s => !s.Success);
        TotalMatches = sourceResults.Where(s => s.Success).Sum(s => s.TotalResults);
    }
}

