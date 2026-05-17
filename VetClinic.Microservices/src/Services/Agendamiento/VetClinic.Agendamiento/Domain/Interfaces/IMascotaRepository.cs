namespace VetClinic.Agendamiento.Domain.Interfaces;

using VetClinic.Agendamiento.Domain.Entities;

/// <summary>
/// Interfaz de repositorio para Mascotas en el contexto de Agendamiento.
/// Definida en Domain, implementada en Infrastructure.
/// </summary>
public interface IMascotaRepository
{
    Task<Mascota?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Mascota>> GetByClienteIdAsync(Guid clienteId, CancellationToken ct = default);
    Task AddAsync(Mascota mascota, CancellationToken ct = default);
}
