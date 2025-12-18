namespace EY.DueDiligenceScreening.API.Shared.Interfaces.Response;

public record ErrorResponse(
    string Code,
    string Message,
    DateTime Timestamp
)
{
    public ErrorResponse(string code, string message) : this(code, message, DateTime.UtcNow)
    {
    }
}

