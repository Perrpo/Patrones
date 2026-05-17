namespace VetClinic.Agendamiento.Application.UseCases;
using VetClinic.Agendamiento.Application.DTOs;
using VetClinic.Agendamiento.Application.Mappings;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.BuildingBlocks.Application;
using VetClinic.BuildingBlocks.Domain;

/// <summary>Caso de uso: Cancelar una cita.</summary>
public class CancelarCitaUseCase
{
    private readonly IAgendaRepository _agendaRepo;
    private readonly IEventPublisher _eventPublisher;

    public CancelarCitaUseCase(IAgendaRepository agendaRepo, IEventPublisher eventPublisher)
    { _agendaRepo = agendaRepo; _eventPublisher = eventPublisher; }

    public async Task<CitaDto> EjecutarAsync(Guid agendaId, Guid citaId, CancellationToken ct = default)
    {
        var agenda = await _agendaRepo.GetByIdAsync(agendaId, ct)
            ?? throw new DomainException("Agenda no encontrada.");
        var cita = agenda.Citas.FirstOrDefault(c => c.Id == citaId)
            ?? throw new DomainException("Cita no encontrada.");

        cita.Cancelar();
        await _agendaRepo.UpdateAsync(agenda, ct);
        await _eventPublisher.PublishAllAsync(cita.DomainEvents, ct);
        cita.ClearDomainEvents();
        return AgendamientoMapper.ToDto(cita);
    }
}
