namespace VetClinic.Agendamiento.Domain.Aggregates;

using VetClinic.Agendamiento.Domain.Entities;
using VetClinic.Agendamiento.Domain.Enums;
using VetClinic.Agendamiento.Domain.ValueObjects;
using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// AGGREGATE ROOT - Raíz del agregado de Agendamiento.
/// Es el único punto de acceso para modificar citas.
/// Asegura la consistencia: no se crucen horarios, no hay conflictos.
/// Unidad de persistencia: se guarda todo el bloque en una transacción.
/// </summary>
public class Agenda : AggregateRoot<Guid>
{
    public Guid ProfesionalId { get; private set; }
    public string NombreProfesional { get; private set; }

    private readonly List<HorarioLaboral> _horariosLaborales = new();
    private readonly List<Cita> _citas = new();

    /// <summary>Acceso de solo lectura a los horarios laborales.</summary>
    public IReadOnlyList<HorarioLaboral> HorariosLaborales => _horariosLaborales.AsReadOnly();

    /// <summary>Acceso de solo lectura a las citas.</summary>
    public IReadOnlyList<Cita> Citas => _citas.AsReadOnly();

    public Agenda(Guid id, Guid profesionalId, string nombreProfesional) : base(id)
    {
        if (profesionalId == Guid.Empty)
            throw new DomainException("El profesional es obligatorio.");
        if (string.IsNullOrWhiteSpace(nombreProfesional))
            throw new DomainException("El nombre del profesional es obligatorio.");

        ProfesionalId = profesionalId;
        NombreProfesional = nombreProfesional;
    }

    // ==================== PUNTO DE ACCESO ÚNICO ====================

    /// <summary>
    /// Agrega una cita validando todas las invariantes del agregado.
    /// Este es el ÚNICO punto de acceso para crear citas (patrón Aggregate Root).
    /// </summary>
    public Cita AgregarCita(HorarioDisponible horario, Guid mascotaId)
    {
        // Invariante 1: El horario debe estar dentro de un turno laboral
        var dentroTurno = _horariosLaborales.Any(h =>
            h.Fecha.Date == horario.Fecha.Date && h.Contiene(horario.HoraInicio, horario.HoraFin));
        if (!dentroTurno)
            throw new DomainException("La cita está fuera del horario laboral del profesional.");

        // Invariante 2: No puede haber conflicto con citas activas existentes
        if (HayConflicto(horario.Fecha, horario.HoraInicio, horario.Duracion))
            throw new DomainException("El profesional ya tiene una cita en ese horario.");

        var cita = new Cita(Guid.NewGuid(), horario, mascotaId, Id);
        _citas.Add(cita);
        return cita;
    }

    /// <summary>Agrega un horario laboral a la agenda.</summary>
    public void AgregarHorarioLaboral(HorarioLaboral horario)
    {
        _horariosLaborales.Add(horario);
    }

    /// <summary>Agrega múltiples horarios laborales.</summary>
    public void AgregarHorariosLaborales(IEnumerable<HorarioLaboral> horarios)
    {
        _horariosLaborales.AddRange(horarios);
    }

    // ==================== INVARIANTES DEL AGREGADO ====================

    /// <summary>
    /// Verifica si hay conflicto de horario con citas activas.
    /// Invariante: no pueden existir dos citas activas que se solapen.
    /// </summary>
    public bool HayConflicto(DateTime fecha, TimeSpan inicio, TimeSpan duracion)
    {
        var fin = inicio + duracion;
        return _citas
            .Where(c => c.EstaActiva())
            .Any(c => c.Horario.Fecha.Date == fecha.Date &&
                !(fin <= c.Horario.HoraInicio || inicio >= c.Horario.HoraFin));
    }

    /// <summary>Obtiene los slots disponibles para una fecha y duración.</summary>
    public List<HorarioDisponible> ObtenerDisponibilidad(DateTime fecha, TimeSpan duracion)
    {
        var horariosDia = _horariosLaborales
            .Where(h => h.Fecha.Date == fecha.Date)
            .OrderBy(h => h.HoraInicio).ToList();

        if (horariosDia.Count == 0) return new();

        var slots = new List<HorarioDisponible>();
        foreach (var turno in horariosDia)
        {
            for (var ini = turno.HoraInicio; ini + duracion <= turno.HoraFin; ini += TimeSpan.FromMinutes(30))
            {
                if (!HayConflicto(fecha, ini, duracion))
                    slots.Add(new HorarioDisponible(fecha, ini, ini + duracion));
            }
        }
        return slots;
    }

    /// <summary>Obtiene las citas de una fecha específica.</summary>
    public List<Cita> ObtenerCitasPorFecha(DateTime fecha) =>
        _citas.Where(c => c.Horario.Fecha.Date == fecha.Date)
              .OrderBy(c => c.Horario.HoraInicio).ToList();

    // Constructor privado para EF Core
    private Agenda() : base(Guid.Empty) { NombreProfesional = ""; }
}
