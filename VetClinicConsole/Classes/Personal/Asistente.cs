namespace VetClinicConsole.Classes.Personal;

using VetClinicConsole.Classes.Agendas;
using VetClinicConsole.Interfaces;

public class Asistente : Empleado, IAgendable
{
    public string Departamento { get; }
    private readonly Agenda _agenda;

    public Asistente(int id, string nombre, string email, string departamento)
        : base(id, nombre, email, "Asistente")
    {
        Departamento = departamento;
        _agenda = new Agenda(id);
    }

    public Agenda ObtenerAgenda() => _agenda;

    public List<HorarioDisponible> VerDisponibilidad(DateTime fecha, TimeSpan duracion) =>
        _agenda.ObtenerDisponibilidad(fecha, duracion);
}
