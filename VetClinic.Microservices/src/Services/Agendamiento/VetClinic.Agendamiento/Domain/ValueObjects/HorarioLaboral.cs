namespace VetClinic.Agendamiento.Domain.ValueObjects;

using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Value Object inmutable que representa un turno laboral de un profesional.
/// Validación estricta en constructor.
/// </summary>
public class HorarioLaboral : ValueObject
{
    public DateTime Fecha { get; }
    public TimeSpan HoraInicio { get; }
    public TimeSpan HoraFin { get; }

    public HorarioLaboral(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
    {
        if (horaFin <= horaInicio)
            throw new DomainException("La hora de fin debe ser mayor a la hora de inicio.");

        Fecha = fecha.Date;
        HoraInicio = horaInicio;
        HoraFin = horaFin;
    }

    /// <summary>Verifica si un rango de tiempo cabe dentro de este turno.</summary>
    public bool Contiene(TimeSpan inicio, TimeSpan fin) =>
        inicio >= HoraInicio && fin <= HoraFin;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Fecha;
        yield return HoraInicio;
        yield return HoraFin;
    }
}
