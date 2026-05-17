namespace VetClinic.Agendamiento.Domain.ValueObjects;
using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Value Object inmutable que representa la duracion de un servicio veterinario.
/// Recibe datos en constructor con lanzamiento de excepciones si son invalidos.
/// No tiene setters publicos.
/// </summary>
public class DuracionServicio : ValueObject
{
    public int Minutos { get; }

    public DuracionServicio(int minutos)
    {
        if (minutos <= 0)
            throw new DomainException("La duracion en minutos debe ser mayor que cero.");
        Minutos = minutos;
    }

    public TimeSpan ComoTimeSpan() => TimeSpan.FromMinutes(Minutos);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Minutos;
    }
}
