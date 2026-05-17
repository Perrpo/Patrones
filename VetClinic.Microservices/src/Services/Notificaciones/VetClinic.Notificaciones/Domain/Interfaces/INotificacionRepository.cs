namespace VetClinic.Notificaciones.Domain.Interfaces;
using VetClinic.Notificaciones.Domain.Entities;
/// <summary>Interfaz definida en Domain, implementada en Infrastructure.</summary>
public interface INotificacionRepository
{
    Task<Notificacion?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Notificacion>> GetPendientesAsync(CancellationToken ct = default);
    Task AddAsync(Notificacion notificacion, CancellationToken ct = default);
    Task UpdateAsync(Notificacion notificacion, CancellationToken ct = default);
}
