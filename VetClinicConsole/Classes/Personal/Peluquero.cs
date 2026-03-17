namespace VetClinicConsole.Classes.Personal;

using VetClinicConsole.Classes.Agendas;
using VetClinicConsole.Interfaces;

public class Peluquero : Empleado, IAgendable
{
    private readonly Agenda _agenda;

    public Peluquero(int id, string nombre, string email)
        : base(id, nombre, email, "Peluquero")
    {
        _agenda = new Agenda(id);
    }

    public Agenda ObtenerAgenda() => _agenda;

    public List<HorarioDisponible> VerDisponibilidad(DateTime fecha, TimeSpan duracion) =>
        _agenda.ObtenerDisponibilidad(fecha, duracion);
}
