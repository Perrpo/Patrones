namespace VetClinic.Notificaciones.Domain.Interfaces;
using VetClinic.Notificaciones.Domain.Entities;
/// <summary>Interfaz para el servicio de envio. Implementada en Infrastructure.</summary>
public interface INotificacionService
{
    Task<bool> EnviarAsync(Notificacion notificacion, CancellationToken ct = default);
}
