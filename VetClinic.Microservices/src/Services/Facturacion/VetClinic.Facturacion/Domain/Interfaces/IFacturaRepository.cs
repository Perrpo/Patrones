namespace VetClinic.Facturacion.Domain.Interfaces;
using VetClinic.Facturacion.Domain.Aggregates;
public interface IFacturaRepository
{
    Task<Factura?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Factura?> GetByCitaIdAsync(Guid citaId, CancellationToken ct = default);
    Task AddAsync(Factura factura, CancellationToken ct = default);
    Task UpdateAsync(Factura factura, CancellationToken ct = default);
}
