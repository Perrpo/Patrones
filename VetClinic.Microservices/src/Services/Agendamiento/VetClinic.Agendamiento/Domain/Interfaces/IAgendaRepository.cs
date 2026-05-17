namespace VetClinic.Agendamiento.Domain.Interfaces;

using VetClinic.Agendamiento.Domain.Aggregates;

/// <summary>
/// Interfaz de repositorio definida en el Dominio.
/// Implementada en la capa de Infrastructure.
/// Retorna entidades y agregados del dominio, no DTOs.
/// </summary>
public interface IAgendaRepository
{
    Task<Agenda?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Agenda?> GetByProfesionalIdAsync(Guid profesionalId, CancellationToken ct = default);
    Task<List<Agenda>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Agenda agenda, CancellationToken ct = default);
    Task UpdateAsync(Agenda agenda, CancellationToken ct = default);
}
