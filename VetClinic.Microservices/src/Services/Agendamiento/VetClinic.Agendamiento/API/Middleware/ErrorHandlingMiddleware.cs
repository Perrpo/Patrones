namespace VetClinic.Agendamiento.API.Middleware;
using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Middleware global para manejo de excepciones.
/// Captura DomainException (400) y Exception generica (500).
/// NO contiene logica de negocio.
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
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Error de dominio");
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error interno");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = "Error interno del servidor." });
        }
    }
}
