namespace VetClinic.Agendamiento.Application.DTOs;
/// <summary>DTO plano para transportar datos de Cita entre capas. Sin comportamiento.</summary>
public record CitaDto(Guid Id, DateTime Fecha, string HoraInicio, string HoraFin, Guid MascotaId, Guid AgendaId, string Estado);
