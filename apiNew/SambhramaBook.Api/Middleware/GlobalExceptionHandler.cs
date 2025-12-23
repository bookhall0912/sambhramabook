using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace SambhramaBook.Api.Middleware;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var isDevelopment = _environment.IsDevelopment();
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            success = false,
            error = new
            {
                code = "INTERNAL_ERROR",
                message = "An error occurred while processing your request",
                details = isDevelopment ? exception.Message : null,
                stackTrace = isDevelopment ? exception.StackTrace : null
            }
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

