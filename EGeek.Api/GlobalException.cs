using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EGeek.Api;

public class GlobalException(ILogger<GlobalException> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, 
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception");

        var problemDetails = new ProblemDetails();

        if (exception is ArgumentException)
        {
            problemDetails.Title = "One or more arguments are invalid";
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Detail = exception.Message;
        }
        else
        {
            problemDetails.Title = "Internal Server Error";
            problemDetails.Detail = "Internal Server Error";
            problemDetails.Status = StatusCodes.Status500InternalServerError;
        }
        
        context.Response.StatusCode = problemDetails.Status.Value;
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}