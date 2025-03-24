using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TeamHub.Infrastructure.Data.Middleware;

/// <summary>
/// Middleware that catches unhandled exceptions and returns a standardized error response.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/>.
    /// </summary>
    /// <param name="next">The next middleware in the HTTP request pipeline.</param>
    /// <param name="logger">Handles logging.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware and handles any unhandled exceptions during request processing.
    /// </summary>
    /// <param name="context">The HTTP context of the current request.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Proceed with the next middleware in the pipeline
            await _next(context); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred!");

            // Return a generic JSON error response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new
            {
                message = ex?.Message,
                statusCode = context.Response.StatusCode,
                error = "Something went wrong. Please contact support."
            });

            await context.Response.WriteAsync(result);
        }
    }
}