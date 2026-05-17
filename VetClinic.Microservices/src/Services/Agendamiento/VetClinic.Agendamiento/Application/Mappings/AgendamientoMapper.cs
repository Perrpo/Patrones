namespace VetClinic.Agendamiento.Application.Mappings;
using VetClinic.Agendamiento.Application.DTOs;
using VetClinic.Agendamiento.Domain.Aggregates;
using VetClinic.Agendamiento.Domain.Entities;
using VetClinic.Agendamiento.Domain.ValueObjects;

/// <summary>
/// Mapeo explicito entre entidades de dominio y DTOs.
/// No contiene logica de negocio ni validaciones.
/// </summary>
public static class AgendamientoMapper
{
    public static CitaDto ToDto(Cita cita) => new(
        cita.Id, cita.Horario.Fecha,
        cita.Horario.HoraInicio.ToString(@"hh\:mm"),
        cita.Horario.HoraFin.ToString(@"hh\:mm"),
        cita.MascotaId, cita.AgendaId, cita.Estado.ToString());

    public static AgendaDto ToDto(Agenda agenda) => new(
        agenda.Id, agenda.ProfesionalId, agenda.NombreProfesional,
        agenda.Citas.Select(ToDto).ToList());

    public static DisponibilidadDto ToDto(HorarioDisponible h) => new(
        h.Fecha, h.HoraInicio.ToString(@"hh\:mm"), h.HoraFin.ToString(@"hh\:mm"));
}
