using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models.Exceptions;

namespace BoxFactoryAPI.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred during request {TraceId}: {Message}",
            httpContext.TraceIdentifier,
            exception.Message);
        
        var statusCode = HttpStatusCode.InternalServerError;
        var title = "An unexpected error occurred.";
        var detail = "We're sorry, an error occurred. Please try again later or contact support.";


        switch (exception)
        {
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                title = "Resource Not Found";
                detail = notFoundException.Message;
                break;
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                title = "Validation Error";
                detail = validationException.Message;
                break;
        }

        httpContext.Response.StatusCode = (int)statusCode;
        httpContext.Response.ContentType = "application/problem+json"; 

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail, 
            Instance = httpContext.Request.Path,
            // Include a TraceId for client-side correlation with server logs
            Extensions = {
                { "traceId", httpContext.TraceIdentifier }
            }
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true; 
    }
}