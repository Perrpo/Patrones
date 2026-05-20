namespace VetClinic.Personal.Domain.ValueObjects;
using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Value Object inmutable que representa la especialidad de un profesional.
/// Valida en constructor. No tiene setters publicos.
/// La comparación es case-insensitive (usa StringComparer.OrdinalIgnoreCase).
/// </summary>
public class Especialidad : ValueObject
{
    private static readonly HashSet<string> EspecialidadesValidas = new(StringComparer.OrdinalIgnoreCase)
    {
        // Especialidades médico-clínicas
        "Medicina General", "Cirugía", "Urgencias",
        "Dermatología", "Odontología", "Nutrición",
        // Servicios de estética
        "Peluquería", "Estética Canina", "Estética Felina",
        // Roles administrativos
        "Recepción", "Administración"
    };

    public string Nombre { get; }

    public Especialidad(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("La especialidad es obligatoria.");
        if (!EspecialidadesValidas.Contains(nombre))
            throw new DomainException($"Especialidad invalida: '{nombre}'. " +
                $"Validas: {string.Join(", ", EspecialidadesValidas)}");
        Nombre = nombre;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Nombre.ToLowerInvariant();
    }
}
