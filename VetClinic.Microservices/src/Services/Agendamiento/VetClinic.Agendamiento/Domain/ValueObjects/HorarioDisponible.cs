namespace VetClinic.Agendamiento.Domain.ValueObjects;

using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Value Object inmutable que representa un slot de tiempo disponible.
/// Recibe datos en constructor con lanzamiento de excepciones si son inválidos.
/// No tiene setters públicos.
/// </summary>
public class HorarioDisponible : ValueObject
{
    public DateTime Fecha { get; }
    public TimeSpan HoraInicio { get; }
    public TimeSpan HoraFin { get; }

    public HorarioDisponible(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
    {
        if (horaFin <= horaInicio)
            throw new DomainException("La hora de fin debe ser mayor a la hora de inicio.");
        if (fecha.Date < DateTime.Today)
            throw new DomainException("La fecha no puede ser anterior a hoy.");

        Fecha = fecha.Date;
        HoraInicio = horaInicio;
        HoraFin = horaFin;
    }

    public DateTime FechaCompleta => Fecha.Date + HoraInicio;
    public TimeSpan Duracion => HoraFin - HoraInicio;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Fecha;
        yield return HoraInicio;
        yield return HoraFin;
    }
}
