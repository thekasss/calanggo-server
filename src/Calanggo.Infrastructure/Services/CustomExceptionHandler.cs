using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Calanggo.Infrastructure.Services;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger, IHostEnvironment hostEnvironment)
    : IExceptionHandler
{
    private readonly IHostEnvironment _hostEnvironment = hostEnvironment;
    private readonly ILogger<CustomExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Ocorreu um erro não tratado: {ErrorMessage}", exception.Message);
        ProblemDetails problemDetails = new()
        {
            Status = 500, Detail = exception.Message, 
            Instance = httpContext.Request.Path
        };
        
        problemDetails.Extensions["exceptionType"] = exception.GetType().Name;

        httpContext.Response.StatusCode = 500;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}