
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pingu.Infrastructure.Services;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger, IHostEnvironment hostEnvironment) : IExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger = logger;
    private readonly IHostEnvironment _hostEnvironment = hostEnvironment;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Ocorreu um erro não tratado: {ErrorMessage}", exception.Message);
        var statusCode = exception switch
        {
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        if (_hostEnvironment.IsDevelopment())
        {
            problemDetails.Extensions["stackTrace"] = exception.StackTrace?.Trim();
            problemDetails.Extensions["exceptionType"] = exception.GetType().Name;
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}