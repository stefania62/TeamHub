using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TeamHub.Infrastructure.Middleware;

/// <summary>
/// Exception handling middleware.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // Proceed to the next middleware/controller
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred!");

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