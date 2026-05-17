namespace VetClinic.Agendamiento.Domain.Interfaces;

using VetClinic.Agendamiento.Domain.Entities;

/// <summary>
/// Interfaz de repositorio para Citas.
/// Definida en Domain, implementada en Infrastructure.
/// </summary>
public interface ICitaRepository
{
    Task<Cita?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Cita>> GetByMascotaIdAsync(Guid mascotaId, CancellationToken ct = default);
    Task<List<Cita>> GetByFechaAsync(DateTime fecha, CancellationToken ct = default);
}
