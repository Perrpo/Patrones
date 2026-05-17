namespace VetClinic.Agendamiento.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Agendamiento.Domain.Entities;

public class MascotaConfiguration : IEntityTypeConfiguration<Mascota>
{
    public void Configure(EntityTypeBuilder<Mascota> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Nombre).HasMaxLength(100).IsRequired();
        builder.Property(m => m.Especie).HasMaxLength(50).IsRequired();
        builder.Property(m => m.Raza).HasMaxLength(50);
        builder.Ignore(m => m.DomainEvents);
    }
}
