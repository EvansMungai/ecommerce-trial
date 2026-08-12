namespace Catalog.API.Extensions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex) { await HandleExceptionAsync(context, ex, _logger, _env); }
    }
    private static Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger logger, IHostEnvironment env)
    {
        logger.LogError(ex, "Unhandled exception occurred while processing request");

        int statusCode = GetStatusCode(ex);
        bool showDetails = env.IsDevelopment() || statusCode == StatusCodes.Status400BadRequest || statusCode == StatusCodes.Status400BadRequest;

        var response = new
        {
            message = GetMessage(statusCode),
            detail = showDetails ? ex.InnerException?.Message ?? ex.Message : null
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsJsonAsync(response);
    }
    private static int GetStatusCode(Exception ex) => ex switch
    {
        ArgumentException => StatusCodes.Status400BadRequest,
        KeyNotFoundException => StatusCodes.Status404NotFound,
        _ => StatusCodes.Status500InternalServerError
    };
    private static string GetMessage(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "The request is invalid.",
        StatusCodes.Status404NotFound => "The requested resource was not found.",
        _ => "An unexpected error occurred."
    };
}
