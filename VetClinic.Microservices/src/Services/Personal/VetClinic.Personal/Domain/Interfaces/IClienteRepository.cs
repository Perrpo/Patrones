namespace VetClinic.Personal.Domain.Interfaces;
using VetClinic.Personal.Domain.Aggregates;
public interface IClienteRepository
{
    Task<Cliente?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Cliente>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Cliente cliente, CancellationToken ct = default);
    Task UpdateAsync(Cliente cliente, CancellationToken ct = default);
}
