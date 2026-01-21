using System.Net;

namespace BuberDinner.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //500;
            context.Response.ContentType = "application/json";

            var errorResult = new
            {
                error = ex.Message
            };

            await context.Response.WriteAsJsonAsync(errorResult);
        }
    }
}