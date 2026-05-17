namespace VetClinic.Agendamiento.Domain.Services;
using VetClinic.Agendamiento.Domain.Aggregates;
using VetClinic.Agendamiento.Domain.ValueObjects;

/// <summary>
/// Servicio de Dominio: calcula slots de tiempo disponibles para un profesional.
/// Considera horarios laborales y citas ya agendadas.
/// No depende de infraestructura.
/// </summary>
public class ServicioDisponibilidad
{
    /// <summary>
    /// Calcula los slots disponibles para un profesional en una fecha.
    /// 1) Solo muestra slots dentro de horarios laborales.
    /// 2) Excluye slots que se solapan con citas existentes.
    /// 3) Los slots se generan en intervalos de 30 minutos.
    /// 4) La duracion del slot depende del servicio solicitado.
    /// </summary>
    public List<HorarioDisponible> CalcularDisponibilidad(
        Agenda agenda, DateTime fecha, DuracionServicio duracion)
    {
        var duracionTimeSpan = duracion.ComoTimeSpan();
        var horariosDia = agenda.HorariosLaborales
            .Where(h => h.Fecha.Date == fecha.Date)
            .OrderBy(h => h.HoraInicio)
            .ToList();

        if (horariosDia.Count == 0)
            return new List<HorarioDisponible>();

        var slots = new List<HorarioDisponible>();
        foreach (var turno in horariosDia)
        {
            // Generar slots en intervalos de 30 minutos dentro del turno
            for (var inicio = turno.HoraInicio;
                 inicio + duracionTimeSpan <= turno.HoraFin;
                 inicio += TimeSpan.FromMinutes(30))
            {
                // Solo agregar si no hay conflicto con citas existentes
                if (!agenda.HayConflicto(fecha, inicio, duracionTimeSpan))
                {
                    slots.Add(new HorarioDisponible(fecha, inicio, inicio + duracionTimeSpan));
                }
            }
        }

        return slots;
    }
}
