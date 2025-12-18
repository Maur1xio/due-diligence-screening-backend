using System.Text.Json.Serialization;

namespace EY.DueDiligenceScreening.API.IAM.Interfaces.REST.Resources;

public record AuthenticatedUserResource(
    [property: JsonPropertyName("userId")] long UserID,
    string Email,
    string FullName,
    string Token,
    DateTime ExpiresAt
);

