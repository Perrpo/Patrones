namespace VetClinic.Notificaciones.Infrastructure.Services;
using VetClinic.Notificaciones.Domain.Entities;
using VetClinic.Notificaciones.Domain.Interfaces;

/// <summary>
/// Servicio de infraestructura: envio de emails.
/// Puramente tecnico, no contiene logica de negocio.
/// En produccion usaria un proveedor real (SendGrid, SES, etc.).
/// </summary>
public class EmailNotificationService : INotificacionService
{
    private readonly ILogger<EmailNotificationService> _logger;
    public EmailNotificationService(ILogger<EmailNotificationService> logger) { _logger = logger; }

    public Task<bool> EnviarAsync(Notificacion notificacion, CancellationToken ct = default)
    {
        _logger.LogInformation(
            "[EMAIL] Para: {Email} | Asunto: {Asunto} | Mensaje: {Mensaje}",
            notificacion.Destinatario.Email,
            notificacion.Asunto,
            notificacion.Mensaje);

        // Simulacion de envio exitoso
        return Task.FromResult(true);
    }
}
