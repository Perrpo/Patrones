namespace VetClinicConsole.Classes.Agendas;

/// <summary>
/// Turno laboral de un profesional en una fecha específica.
/// </summary>
public record HorarioLaboral(int Id, DateTime Fecha, TimeSpan HoraInicio, TimeSpan HoraFin)
{
    public bool Contiene(TimeSpan inicio, TimeSpan fin) =>
        inicio >= HoraInicio && fin <= HoraFin;
}
