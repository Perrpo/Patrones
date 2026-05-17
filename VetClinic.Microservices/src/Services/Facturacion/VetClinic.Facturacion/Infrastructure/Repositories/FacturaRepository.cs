namespace VetClinic.Facturacion.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using VetClinic.Facturacion.Domain.Aggregates;
using VetClinic.Facturacion.Domain.Interfaces;
using VetClinic.Facturacion.Infrastructure.Persistence;

public class FacturaRepository : IFacturaRepository
{
    private readonly FacturacionDbContext _ctx;
    public FacturaRepository(FacturacionDbContext ctx) => _ctx = ctx;
    public async Task<Factura?> GetByIdAsync(Guid id, CancellationToken ct) => await _ctx.Facturas.Include(f => f.Pagos).FirstOrDefaultAsync(f => f.Id == id, ct);
    public async Task<Factura?> GetByCitaIdAsync(Guid citaId, CancellationToken ct) => await _ctx.Facturas.Include(f => f.Pagos).FirstOrDefaultAsync(f => f.CitaId == citaId, ct);
    public async Task AddAsync(Factura f, CancellationToken ct) { await _ctx.Facturas.AddAsync(f, ct); await _ctx.SaveChangesAsync(ct); }
    public async Task UpdateAsync(Factura f, CancellationToken ct) { _ctx.Facturas.Update(f); await _ctx.SaveChangesAsync(ct); }
}
