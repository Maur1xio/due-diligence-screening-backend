using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EY.DueDiligenceScreening.API.Shared.Interfaces.Response;

namespace EY.DueDiligenceScreening.API.Shared.Interfaces.Filters;


public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var statusCode = context.Exception switch
        {
            ArgumentException => 400,
            InvalidOperationException => 400,
            UnauthorizedAccessException => 401,
            KeyNotFoundException => 404,
            _ => 500
        };

        var errorCode = context.Exception switch
        {
            ArgumentException => "VALIDATION_ERROR",
            InvalidOperationException => "INVALID_OPERATION",
            UnauthorizedAccessException => "UNAUTHORIZED",
            KeyNotFoundException => "NOT_FOUND",
            _ => "INTERNAL_ERROR"
        };

        _logger.LogError(context.Exception, 
            "Unhandled exception: {ExceptionType} - {Message}", 
            context.Exception.GetType().Name, 
            context.Exception.Message);

        var response = new ApiErrorResponse(
            statusCode,
            GetUserFriendlyMessage(statusCode),
            errorCode,
            context.Exception.Message
        );

        context.Result = new ObjectResult(response)
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
    }

    private static string GetUserFriendlyMessage(int statusCode)
    {
        return statusCode switch
        {
            400 => "The request contains invalid data",
            401 => "Authentication is required",
            403 => "You don't have permission to access this resource",
            404 => "The requested resource was not found",
            500 => "An internal server error occurred",
            _ => "An error occurred while processing your request"
        };
    }
}

