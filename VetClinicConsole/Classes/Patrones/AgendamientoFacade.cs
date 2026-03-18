namespace VetClinicConsole.Classes.Patrones;

using VetClinicConsole.Classes.Agendas;
using VetClinicConsole.Classes.Citas;
using VetClinicConsole.Interfaces;

public class AgendamientoFacade
{
    private readonly List<Cita> _citas;
    private readonly CitaFactory _factory;

    public AgendamientoFacade(List<Cita> citas, CitaFactory factory)
    {
        _citas = citas;
        _factory = factory;
    }

    public string AgendarCita(DateTime fechaHora, Mascota mascota, IAgendable profesional, IEnumerable<IServicio> servicios)
    {
        var duracion = TimeSpan.FromMinutes(servicios.Sum(s => s.Duracion()));
        var inicio = fechaHora.TimeOfDay;
        var fin = inicio.Add(duracion);
        var jornadaInicio = TimeSpan.FromHours(8);
        var jornadaFin = TimeSpan.FromHours(18);

        if (fechaHora.Date < DateTime.Today)
            return "La fecha no puede ser anterior a hoy.";

        if (inicio < jornadaInicio || fin > jornadaFin)
            return "Solo se permiten citas entre las 08:00 AM y las 18:00 PM.";

        var traslapaMascota = _citas.Any(c =>
            c.Mascota.Id == mascota.Id &&
            c.Horario.Fecha.Date == fechaHora.Date &&
            !(fin <= c.Horario.HoraInicio || inicio >= c.Horario.HoraFin));
        if (traslapaMascota)
            return "La mascota ya tiene una cita en ese horario.";

        var horario = new HorarioDisponible(fechaHora.Date, inicio, fin);
        var cita = _factory.CrearCita(horario, mascota, profesional, servicios);
        _citas.Add(cita);
        return $"Cita {cita.Id} agendada para {mascota.Nombre} con {profesional.ObtenerAgenda().Citas.Count} citas en agenda.";
    }

    public bool CancelarCita(int idCita)
    {
        var cita = _citas.FirstOrDefault(c => c.Id == idCita);
        if (cita is null) return false;
        cita.Cancelar();
        return true;
    }

    public List<Cita> ConsultarAgenda(DateTime fecha) =>
        _citas.Where(c => c.Horario.Fecha.Date == fecha.Date).OrderBy(c => c.Horario.HoraInicio).ToList();
}
