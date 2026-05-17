namespace VetClinic.Agendamiento.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Agendamiento.Domain.Entities;

public class CitaConfiguration : IEntityTypeConfiguration<Cita>
{
    public void Configure(EntityTypeBuilder<Cita> builder)
    {
        builder.HasKey(c => c.Id);
        builder.OwnsOne(c => c.Horario);
        builder.Property(c => c.Estado).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.MascotaId).IsRequired();
        builder.Ignore(c => c.DomainEvents);
    }
}
