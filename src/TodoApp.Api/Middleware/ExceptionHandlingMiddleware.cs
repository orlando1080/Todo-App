using System.Text.Json;
using ToDoApp.Application.Errors;

namespace TodoApp.Api.Middleware;

internal sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation(ex, ex.Message);

            string json = JsonSerializer.Serialize(new
            {
                statusCode = 404,
                message = ex.Message,
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(json).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            string json = JsonSerializer.Serialize(new
            {
                statusCode = 500,
                message = "Internal Server Error",
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(json).ConfigureAwait(false);
        }
    }
}