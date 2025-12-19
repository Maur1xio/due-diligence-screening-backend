using EY.DueDiligenceScreening.API.Screening.Domain.Model.ValueObjects;

namespace EY.DueDiligenceScreening.API.Screening.Domain.Model.Entities;

/// <summary>
/// Resultado de un screening en una fuente específica
/// </summary>
public class SourceResult
{
    public ScreeningSource Source { get; set; }
    public int TotalResults { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public object Data { get; set; } = new();

    public SourceResult() { }

    public SourceResult(
        ScreeningSource source,
        int totalResults,
        bool success,
        object data,
        TimeSpan executionTime,
        string? errorMessage = null)
    {
        Source = source;
        TotalResults = totalResults;
        Success = success;
        Data = data;
        ExecutionTime = executionTime;
        ErrorMessage = errorMessage;
    }
}

