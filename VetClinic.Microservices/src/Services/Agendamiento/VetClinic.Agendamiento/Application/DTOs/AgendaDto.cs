namespace VetClinic.Agendamiento.Application.DTOs;
/// <summary>DTO plano para transportar datos de Agenda.</summary>
public record AgendaDto(Guid Id, Guid ProfesionalId, string NombreProfesional, List<CitaDto> Citas);
