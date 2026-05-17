namespace VetClinic.ApiGateway.Middleware;

/// <summary>
/// Middleware que registra todas las peticiones entrantes al Gateway.
/// Captura metodo HTTP, ruta, y tiempo de respuesta.
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    { _next = next; _logger = logger; }

    public async Task InvokeAsync(HttpContext context)
    {
        var inicio = DateTime.UtcNow;
        _logger.LogInformation("[Gateway] {Method} {Path} - Inicio", context.Request.Method, context.Request.Path);

        await _next(context);

        var duracion = DateTime.UtcNow - inicio;
        _logger.LogInformation("[Gateway] {Method} {Path} - {StatusCode} ({Duracion}ms)",
            context.Request.Method, context.Request.Path, context.Response.StatusCode, duracion.TotalMilliseconds);
    }
}
