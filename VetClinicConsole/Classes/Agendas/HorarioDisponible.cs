namespace VetClinicConsole.Classes.Agendas;

public record HorarioDisponible(DateTime Fecha, TimeSpan HoraInicio, TimeSpan HoraFin)
{
    public DateTime FechaCompleta => Fecha.Date + HoraInicio;
}
