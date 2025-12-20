using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EY.DueDiligenceScreening.API.Shared.Interfaces.Response;

namespace EY.DueDiligenceScreening.API.Shared.Infrastructure.Extensions;

public static class JwtBearerExtensions
{
    public static void ConfigureCustomEvents(this JwtBearerOptions options)
    {
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                // Prevenir el comportamiento por defecto
                context.HandleResponse();
                
                // Crear respuesta personalizada para 401
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                
                var errorResponse = new ApiErrorResponse(
                    401,
                    "Authentication is required to access this resource",
                    "UNAUTHORIZED",
                    "No valid authentication token was provided or the token has expired"
                );
                
                var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                
                return context.Response.WriteAsync(json);
            },
            
            OnForbidden = context =>
            {
                // Crear respuesta personalizada para 403
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                
                var errorResponse = new ApiErrorResponse(
                    403,
                    "You don't have permission to access this resource",
                    "FORBIDDEN",
                    "Your account does not have the required permissions"
                );
                
                var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                
                return context.Response.WriteAsync(json);
            }
        };
    }
}

