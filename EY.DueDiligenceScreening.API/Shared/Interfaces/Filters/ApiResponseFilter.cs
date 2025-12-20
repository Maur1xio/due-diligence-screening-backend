using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EY.DueDiligenceScreening.API.Shared.Interfaces.Response;

namespace EY.DueDiligenceScreening.API.Shared.Interfaces.Filters;

public class ApiResponseFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult)
        {
            var statusCode = objectResult.StatusCode ?? 200;
            
            if (objectResult.Value is ApiResponse<object> || 
                objectResult.Value is ApiErrorResponse)
            {
                return;
            }

            if (objectResult.Value is ErrorResponse errorResponse)
            {
                objectResult.Value = new ApiErrorResponse(
                    statusCode,
                    errorResponse.Message,
                    errorResponse.Code,
                    errorResponse.Message
                );
                return;
            }

            if (statusCode >= 200 && statusCode < 300)
            {
                var message = GetSuccessMessage(context, statusCode);
                objectResult.Value = new ApiResponse<object>(
                    objectResult.Value!,
                    message,
                    statusCode
                );
            }
            else
            {
                var cleanErrors = CleanErrorObject(objectResult.Value);
                var errorMessage = GetErrorMessage(statusCode);
                
                objectResult.Value = new ApiErrorResponse(
                    statusCode,
                    errorMessage,
                    cleanErrors
                );
            }
        }
    }

    private static object? CleanErrorObject(object? errorValue)
    {
        if (errorValue == null) return null;

        var errorType = errorValue.GetType();
        
        var errorsProperty = errorType.GetProperty("Errors");
        if (errorsProperty != null)
        {
            var errors = errorsProperty.GetValue(errorValue);
            if (errors != null)
            {
                return errors; 
            }
        }

        return errorValue;
    }

    private static string GetErrorMessage(int statusCode)
    {
        return statusCode switch
        {
            400 => "The request contains invalid data",
            401 => "Authentication is required to access this resource",
            403 => "You don't have permission to access this resource",
            404 => "The requested resource was not found",
            500 => "An internal server error occurred",
            _ => "An error occurred while processing your request"
        };
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }

    private static string GetSuccessMessage(ResultExecutingContext context, int statusCode)
    {
        return statusCode switch
        {
            201 => "Resource created successfully",
            204 => "Operation completed successfully",
            _ => "Operation completed successfully"
        };
    }
}

