namespace VetClinic.Agendamiento.Domain.Entities;

using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Entidad que representa una mascota en el contexto de Agendamiento.
/// Tiene identidad propia y validación en constructor.
/// </summary>
public class Mascota : Entity<Guid>
{
    public string Nombre { get; private set; }
    public string Especie { get; private set; }
    public string Raza { get; private set; }
    public int Edad { get; private set; }
    public Guid ClienteId { get; private set; }

    public Mascota(Guid id, string nombre, string especie, string raza, int edad, Guid clienteId)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre de la mascota es obligatorio.");
        if (string.IsNullOrWhiteSpace(especie))
            throw new DomainException("La especie es obligatoria.");
        if (edad < 0)
            throw new DomainException("La edad no puede ser negativa.");

        Nombre = nombre;
        Especie = especie;
        Raza = raza ?? string.Empty;
        Edad = edad;
        ClienteId = clienteId;
    }

    /// <summary>Actualiza la información de la mascota.</summary>
    public void ActualizarInfo(string nombre, string especie, string raza, int edad)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre es obligatorio.");
        Nombre = nombre;
        Especie = especie;
        Raza = raza;
        Edad = edad;
    }

    // Constructor privado para EF Core (requerido por el ORM)
    private Mascota() : base(Guid.Empty) { Nombre = ""; Especie = ""; Raza = ""; }
}
