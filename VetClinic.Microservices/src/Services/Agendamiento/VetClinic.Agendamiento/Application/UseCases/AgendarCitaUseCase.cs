namespace VetClinic.Agendamiento.Application.UseCases;
using VetClinic.Agendamiento.Application.DTOs;
using VetClinic.Agendamiento.Application.Mappings;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.Agendamiento.Domain.Services;
using VetClinic.Agendamiento.Domain.ValueObjects;
using VetClinic.BuildingBlocks.Application;
using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Caso de uso: Agendar una nueva cita.
/// Orquesta la operacion: obtiene datos, ejecuta dominio, persiste, publica eventos.
/// NO contiene logica de negocio (esa esta en el Agregado y ServicioAgendamiento).
/// </summary>
public class AgendarCitaUseCase
{
    private readonly IAgendaRepository _agendaRepo;
    private readonly ICitaRepository _citaRepo;
    private readonly ServicioAgendamiento _servicioAgendamiento;
    private readonly IEventPublisher _eventPublisher;

    public AgendarCitaUseCase(IAgendaRepository agendaRepo, ICitaRepository citaRepo,
        ServicioAgendamiento servicioAgendamiento, IEventPublisher eventPublisher)
    {
        _agendaRepo = agendaRepo;
        _citaRepo = citaRepo;
        _servicioAgendamiento = servicioAgendamiento;
        _eventPublisher = eventPublisher;
    }

    public async Task<CitaDto> EjecutarAsync(CrearCitaRequest request, CancellationToken ct = default)
    {
        // 1. Obtener agregado raiz
        var agenda = await _agendaRepo.GetByProfesionalIdAsync(request.ProfesionalId, ct)
            ?? throw new DomainException("No se encontro agenda para el profesional.");

        // 2. Crear Value Object (valida en constructor - lanza excepcion si es invalido)
        var horario = new HorarioDisponible(request.Fecha, request.HoraInicio, request.HoraFin);

        // 3. Validar con servicio de dominio (logica que cruza agregados)
        var citasMascota = await _citaRepo.GetByMascotaIdAsync(request.MascotaId, ct);
        _servicioAgendamiento.ValidarDisponibilidad(agenda, request.MascotaId, horario, citasMascota);

        // 4. Ejecutar logica de negocio a traves del agregado (punto de acceso unico)
        var cita = agenda.AgregarCita(horario, request.MascotaId);

        // 5. Persistir (transaccion)
        await _agendaRepo.UpdateAsync(agenda, ct);

        // 6. Publicar eventos de dominio
        await _eventPublisher.PublishAllAsync(cita.DomainEvents, ct);
        cita.ClearDomainEvents();

        // 7. Retornar DTO (nunca exponer entidad de dominio)
        return AgendamientoMapper.ToDto(cita);
    }
}
