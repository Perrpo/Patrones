namespace VetClinic.Agendamiento.Application.UseCases;
using VetClinic.Agendamiento.Application.DTOs;
using VetClinic.Agendamiento.Application.Mappings;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.BuildingBlocks.Domain;

/// <summary>Caso de uso (lectura): Consultar la agenda de un profesional.</summary>
public class ConsultarAgendaUseCase
{
    private readonly IAgendaRepository _agendaRepo;
    public ConsultarAgendaUseCase(IAgendaRepository agendaRepo) { _agendaRepo = agendaRepo; }

    public async Task<AgendaDto> EjecutarAsync(Guid profesionalId, CancellationToken ct = default)
    {
        var agenda = await _agendaRepo.GetByProfesionalIdAsync(profesionalId, ct)
            ?? throw new DomainException("Agenda no encontrada.");
        return AgendamientoMapper.ToDto(agenda);
    }
}
