namespace VetClinic.Personal.Infrastructure.Repositories;

using VetClinic.Personal.Domain.Aggregates;
using VetClinic.Personal.Domain.Interfaces;

/// <summary>
/// Repositorio en memoria de Clientes.
/// En producción se reemplazaría por una implementación con EF Core.
/// </summary>
public class InMemoryClienteRepository : IClienteRepository
{
    private readonly List<Cliente> _clientes = new();

    public Task<Cliente?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_clientes.FirstOrDefault(c => c.Id == id));

    public Task<List<Cliente>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult(_clientes.ToList());

    public Task AddAsync(Cliente cliente, CancellationToken ct = default)
    {
        _clientes.Add(cliente);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Cliente cliente, CancellationToken ct = default)
        => Task.CompletedTask; // referencia directa en memoria
}
