namespace VetClinic.Facturacion.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using VetClinic.Facturacion.Domain.Aggregates;
public class FacturacionDbContext : DbContext
{
    public DbSet<Factura> Facturas => Set<Factura>();
    public FacturacionDbContext(DbContextOptions<FacturacionDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder m) { m.ApplyConfigurationsFromAssembly(typeof(FacturacionDbContext).Assembly); }
}
