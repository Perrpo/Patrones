namespace VetClinic.Personal.Domain.Entities;
using VetClinic.BuildingBlocks.Domain;
using VetClinic.Personal.Domain.ValueObjects;

/// <summary>
/// Entidad enriquecida: Profesional veterinario.
/// Usa Value Objects (Email, Especialidad) en vez de tipos primitivos.
/// </summary>
public class Profesional : Entity<Guid>
{
    public string Nombre { get; private set; }
    public Email Email { get; private set; }
    public string Rol { get; private set; }
    public Especialidad? Especialidad { get; private set; }

    public Profesional(Guid id, string nombre, Email email, string rol, Especialidad? especialidad = null) : base(id)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(rol))
            throw new DomainException("El rol es obligatorio.");

        Nombre = nombre;
        Email = email;
        Rol = rol;
        Especialidad = especialidad;
    }

    /// <summary>Actualiza la especialidad del profesional.</summary>
    public void AsignarEspecialidad(Especialidad especialidad)
    {
        Especialidad = especialidad ?? throw new DomainException("La especialidad no puede ser nula.");
    }

    private Profesional() : base(Guid.Empty) { Nombre = ""; Email = null!; Rol = ""; }
}
