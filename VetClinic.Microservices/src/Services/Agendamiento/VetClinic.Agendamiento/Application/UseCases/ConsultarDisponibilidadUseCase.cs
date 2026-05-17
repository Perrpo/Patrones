namespace VetClinic.Agendamiento.Application.UseCases;
using VetClinic.Agendamiento.Application.DTOs;
using VetClinic.Agendamiento.Application.Mappings;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.BuildingBlocks.Domain;

/// <summary>Caso de uso (lectura): Consultar disponibilidad de un profesional.</summary>
public class ConsultarDisponibilidadUseCase
{
    private readonly IAgendaRepository _agendaRepo;
    public ConsultarDisponibilidadUseCase(IAgendaRepository agendaRepo) { _agendaRepo = agendaRepo; }

    public async Task<List<DisponibilidadDto>> EjecutarAsync(Guid profesionalId, DateTime fecha, int duracionMinutos, CancellationToken ct = default)
    {
        var agenda = await _agendaRepo.GetByProfesionalIdAsync(profesionalId, ct)
            ?? throw new DomainException("Agenda no encontrada.");
        var slots = agenda.ObtenerDisponibilidad(fecha, TimeSpan.FromMinutes(duracionMinutos));
        return slots.Select(AgendamientoMapper.ToDto).ToList();
    }
}
