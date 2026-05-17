namespace VetClinic.Agendamiento.Application.DTOs;
/// <summary>DTO de entrada para crear una cita. Validado en boundaries.</summary>
public record CrearCitaRequest(Guid ProfesionalId, Guid MascotaId, DateTime Fecha, TimeSpan HoraInicio, TimeSpan HoraFin);
