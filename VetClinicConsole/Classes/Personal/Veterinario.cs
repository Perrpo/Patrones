namespace VetClinicConsole.Classes.Personal;

using VetClinicConsole.Classes.Agendas;
using VetClinicConsole.Interfaces;

public class Veterinario : Empleado, IAgendable
{
    public string Especialidad { get; }
    private readonly Agenda _agenda;

    public Veterinario(int id, string nombre, string email, string especialidad)
        : base(id, nombre, email, "Veterinario")
    {
        Especialidad = especialidad;
        _agenda = new Agenda(id);
    }

    public Agenda ObtenerAgenda() => _agenda;

    public List<HorarioDisponible> VerDisponibilidad(DateTime fecha, TimeSpan duracion) =>
        _agenda.ObtenerDisponibilidad(fecha, duracion);
}
