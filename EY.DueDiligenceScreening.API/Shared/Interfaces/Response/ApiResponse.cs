namespace EY.DueDiligenceScreening.API.Shared.Interfaces.Response;

public record ApiResponse<T>
{
    public bool Success { get; init; }
    public int StatusCode { get; init; }
    public string Message { get; init; }
    public T? Data { get; init; }
    public DateTime Timestamp { get; init; }

    public ApiResponse(T data, string message = "Operation completed successfully", int statusCode = 200)
    {
        Success = true;
        StatusCode = statusCode;
        Message = message;
        Data = data;
        Timestamp = DateTime.UtcNow;
    }

    public ApiResponse(string message, int statusCode = 200)
    {
        Success = true;
        StatusCode = statusCode;
        Message = message;
        Data = default;
        Timestamp = DateTime.UtcNow;
    }
}

