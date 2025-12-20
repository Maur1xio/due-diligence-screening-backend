namespace EY.DueDiligenceScreening.API.Shared.Interfaces.Response;

/// <summary>
/// Estructura estándar para respuestas de error de la API
/// </summary>
public record ApiErrorResponse
{
    public bool Success { get; init; } = false;
    public int StatusCode { get; init; }
    public string Message { get; init; }
    public object? Errors { get; init; }
    public DateTime Timestamp { get; init; }

    public ApiErrorResponse(int statusCode, string message, object? errors = null)
    {
        StatusCode = statusCode;
        Message = message;
        Errors = errors;
        Timestamp = DateTime.UtcNow;
    }

    public ApiErrorResponse(int statusCode, string message, string errorCode, string errorDetails)
    {
        StatusCode = statusCode;
        Message = message;
        Errors = new { code = errorCode, details = errorDetails };
        Timestamp = DateTime.UtcNow;
    }
}

