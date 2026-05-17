namespace VetClinic.ApiGateway.Middleware;

/// <summary>
/// Middleware global de manejo de errores en el Gateway.
/// Captura excepciones y retorna respuestas HTTP apropiadas.
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    { _next = next; _logger = logger; }

    public async Task InvokeAsync(HttpContext context)
    {
        try { await _next(context); }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error comunicandose con microservicio");
            context.Response.StatusCode = 502;
            await context.Response.WriteAsJsonAsync(new { error = "Servicio no disponible." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error interno del gateway");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = "Error interno del servidor." });
        }
    }
}
