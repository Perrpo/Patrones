namespace VetClinic.Agendamiento.Application.DTOs;
/// <summary>DTO para representar un slot de disponibilidad.</summary>
public record DisponibilidadDto(DateTime Fecha, string HoraInicio, string HoraFin);
