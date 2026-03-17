namespace VetClinicConsole.Classes.Agendas;

using VetClinicConsole.Classes.Citas;

public class Agenda
{
    public int Id { get; }
    private readonly List<Cita> _citas = new();

    public IReadOnlyList<Cita> Citas => _citas.AsReadOnly();

    public Agenda(int id)
    {
        Id = id;
    }

    public void AgregarCita(Cita cita)
    {
        _citas.Add(cita);
    }

    public List<Cita> ObtenerCitas(DateTime fecha) =>
        _citas.Where(c => c.Horario.Fecha.Date == fecha.Date).OrderBy(c => c.Horario.HoraInicio).ToList();

    public List<HorarioDisponible> ObtenerDisponibilidad(DateTime fecha, TimeSpan duracion)
    {
        var citasDia = ObtenerCitas(fecha);
        var slots = new List<HorarioDisponible>();
        var horaInicioJornada = TimeSpan.FromHours(8);
        var horaFinJornada = TimeSpan.FromHours(18);

        for (var inicio = horaInicioJornada; inicio + duracion <= horaFinJornada; inicio += TimeSpan.FromMinutes(30))
        {
            var fin = inicio + duracion;
            var traslapa = citasDia.Any(c =>
                !(fin <= c.Horario.HoraInicio || inicio >= c.Horario.HoraFin));
            if (!traslapa)
            {
                slots.Add(new HorarioDisponible(fecha.Date, inicio, fin));
            }
        }

        return slots;
    }
}
