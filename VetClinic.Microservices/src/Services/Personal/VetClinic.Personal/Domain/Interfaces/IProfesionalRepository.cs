namespace VetClinic.Personal.Domain.Interfaces;
using VetClinic.Personal.Domain.Entities;
public interface IProfesionalRepository
{
    Task<Profesional?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Profesional>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Profesional profesional, CancellationToken ct = default);
}
