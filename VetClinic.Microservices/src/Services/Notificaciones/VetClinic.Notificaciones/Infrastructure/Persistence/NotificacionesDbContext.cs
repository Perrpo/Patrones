namespace VetClinic.Notificaciones.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using VetClinic.Notificaciones.Domain.Entities;

public class NotificacionesDbContext : DbContext
{
    public DbSet<Notificacion> Notificaciones => Set<Notificacion>();
    public NotificacionesDbContext(DbContextOptions<NotificacionesDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notificacion>(b =>
        {
            b.HasKey(n => n.Id);
            b.OwnsOne(n => n.Destinatario);
            b.Property(n => n.Estado).HasConversion<string>();
            b.Property(n => n.Tipo).HasConversion<string>();
            b.Ignore(n => n.DomainEvents);
        });
    }
}
