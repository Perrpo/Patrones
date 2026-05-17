namespace VetClinic.Agendamiento.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using VetClinic.Agendamiento.Domain.Aggregates;
using VetClinic.Agendamiento.Domain.Entities;

/// <summary>
/// DbContext del ORM (Entity Framework Core).
/// Solo existe en la capa de Infrastructure, el Dominio NO depende de el.
/// </summary>
public class AgendamientoDbContext : DbContext
{
    public DbSet<Agenda> Agendas => Set<Agenda>();
    public DbSet<Cita> Citas => Set<Cita>();
    public DbSet<Mascota> Mascotas => Set<Mascota>();

    public AgendamientoDbContext(DbContextOptions<AgendamientoDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agenda>(b =>
        {
            b.HasKey(a => a.Id);
            b.Property(a => a.NombreProfesional);
            b.Ignore(a => a.HorariosLaborales);
            b.Ignore(a => a.Citas);
            b.Ignore(a => a.DomainEvents);
        });

        modelBuilder.Entity<Cita>(b =>
        {
            b.HasKey(c => c.Id);
            b.Ignore(c => c.Horario);
            b.Ignore(c => c.DomainEvents);
        });

        modelBuilder.Entity<Mascota>(b =>
        {
            b.HasKey(m => m.Id);
            b.Ignore(m => m.DomainEvents);
        });
    }
}
