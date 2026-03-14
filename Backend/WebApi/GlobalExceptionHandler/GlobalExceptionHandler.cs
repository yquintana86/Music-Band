using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

namespace WebApi.GlobalExceptionHandler;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        Microsoft.AspNetCore.Mvc.ProblemDetails problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "Internal Server Error",
            Title = "Exception",
            Detail = "Url"
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
