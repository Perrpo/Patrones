namespace VetClinic.Personal.Domain.ValueObjects;
using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Value Object inmutable que representa la especialidad de un profesional.
/// Valida en constructor. No tiene setters publicos.
/// </summary>
public class Especialidad : ValueObject
{
    private static readonly HashSet<string> EspecialidadesValidas = new()
    {
        "medicina_general", "cirugia", "urgencias", "dermatologia",
        "odontologia", "peluqueria", "nutricion"
    };

    public string Nombre { get; }

    public Especialidad(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("La especialidad es obligatoria.");
        if (!EspecialidadesValidas.Contains(nombre.ToLower()))
            throw new DomainException($"Especialidad invalida: {nombre}. Validas: {string.Join(", ", EspecialidadesValidas)}");
        Nombre = nombre.ToLower();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Nombre;
    }
}
