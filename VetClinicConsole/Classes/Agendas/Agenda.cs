namespace VetClinicConsole.Classes.Agendas;

using VetClinicConsole.Classes.Citas;

public class Agenda
{
    public int Id { get; }
    private readonly List<HorarioLaboral> _horariosLaborales = new();
    private readonly List<Cita> _citas = new();

    public IReadOnlyList<HorarioLaboral> HorariosLaborales => _horariosLaborales.AsReadOnly();
    public IReadOnlyList<Cita> Citas => _citas.AsReadOnly();

    public Agenda(int id)
    {
        Id = id;
    }

    public void AgregarHorarioLaboral(HorarioLaboral horario) => _horariosLaborales.Add(horario);

    public void AgregarHorariosLaborales(IEnumerable<HorarioLaboral> horarios) => _horariosLaborales.AddRange(horarios);

    public void AgregarCita(Cita cita)
    {
        _citas.Add(cita);
    }

    public List<Cita> ObtenerCitas(DateTime fecha) =>
        _citas.Where(c => c.Horario.Fecha.Date == fecha.Date).OrderBy(c => c.Horario.HoraInicio).ToList();

    public bool HayConflicto(DateTime fecha, TimeSpan inicio, TimeSpan duracion)
    {
        var fin = inicio + duracion;
        return ObtenerCitas(fecha).Any(c =>
            !(fin <= c.Horario.HoraInicio || inicio >= c.Horario.HoraFin));
    }

    public List<HorarioDisponible> ObtenerDisponibilidad(DateTime fecha, TimeSpan duracion)
    {
        var horariosDia = _horariosLaborales
            .Where(h => h.Fecha.Date == fecha.Date)
            .OrderBy(h => h.HoraInicio)
            .ToList();

        if (horariosDia.Count == 0)
        {
            return new List<HorarioDisponible>();
        }

        var citasDia = ObtenerCitas(fecha);
        var slots = new List<HorarioDisponible>();

        foreach (var turno in horariosDia)
        {
            for (var inicio = turno.HoraInicio; inicio + duracion <= turno.HoraFin; inicio += TimeSpan.FromMinutes(30))
            {
                var fin = inicio + duracion;
                var traslapa = citasDia.Any(c =>
                    !(fin <= c.Horario.HoraInicio || inicio >= c.Horario.HoraFin));
                if (!traslapa)
                {
                    slots.Add(new HorarioDisponible(fecha.Date, inicio, fin));
                }
            }
        }

        return slots;
    }
}
