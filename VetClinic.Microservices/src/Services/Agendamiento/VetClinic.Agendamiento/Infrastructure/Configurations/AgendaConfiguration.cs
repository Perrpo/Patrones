namespace VetClinic.Agendamiento.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Agendamiento.Domain.Aggregates;

public class AgendaConfiguration : IEntityTypeConfiguration<Agenda>
{
    public void Configure(EntityTypeBuilder<Agenda> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.ProfesionalId).IsRequired();
        builder.Property(a => a.NombreProfesional).HasMaxLength(200).IsRequired();
        builder.HasMany(a => a.Citas).WithOne().HasForeignKey(c => c.AgendaId);
        builder.Navigation(a => a.Citas).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Ignore(a => a.HorariosLaborales);
        builder.Ignore(a => a.DomainEvents);
    }
}
