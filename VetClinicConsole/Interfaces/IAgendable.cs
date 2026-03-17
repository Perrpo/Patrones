namespace VetClinicConsole.Interfaces;

using VetClinicConsole.Classes.Agendas;

public interface IAgendable
{
    Agenda ObtenerAgenda();
    List<HorarioDisponible> VerDisponibilidad(DateTime fecha, TimeSpan duracion);
}
