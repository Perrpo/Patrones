namespace VetClinic.Agendamiento.Domain.Entities;

using VetClinic.Agendamiento.Domain.Enums;
using VetClinic.Agendamiento.Domain.Events;
using VetClinic.Agendamiento.Domain.ValueObjects;
using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Entidad ENRIQUECIDA que representa una cita veterinaria.
/// Contiene lógica de negocio: transiciones de estado con validación.
/// No es un modelo anémico (tiene comportamiento, no solo getters/setters).
/// </summary>
public class Cita : Entity<Guid>
{
    public HorarioDisponible Horario { get; private set; }
    public Guid MascotaId { get; private set; }
    public Guid AgendaId { get; private set; }
    public EstadoCita Estado { get; private set; }

    // ---------- Diseño por Contrato: Precondiciones en constructor ----------
    public Cita(Guid id, HorarioDisponible horario, Guid mascotaId, Guid agendaId)
        : base(id)
    {
        if (horario is null)
            throw new DomainException("El horario es obligatorio.");
        if (mascotaId == Guid.Empty)
            throw new DomainException("La mascota es obligatoria.");
        if (agendaId == Guid.Empty)
            throw new DomainException("La agenda es obligatoria.");

        Horario = horario;
        MascotaId = mascotaId;
        AgendaId = agendaId;
        Estado = EstadoCita.Pendiente;

        // Evento de dominio elevado desde la entidad
        AddDomainEvent(new CitaCreadaEvent(Id, mascotaId, agendaId, horario.FechaCompleta));
    }

    // ---------- Comportamientos de negocio (Entidad Enriquecida) ----------

    /// <summary>
    /// Confirma la cita. Solo se pueden confirmar citas pendientes.
    /// Postcondición: Estado == Confirmada y se eleva CitaConfirmadaEvent.
    /// </summary>
    public void Confirmar()
    {
        if (Estado != EstadoCita.Pendiente)
            throw new DomainException("Solo se pueden confirmar citas en estado Pendiente.");

        Estado = EstadoCita.Confirmada;
        AddDomainEvent(new CitaConfirmadaEvent(Id, Horario.FechaCompleta));
    }

    /// <summary>
    /// Cancela la cita. No se pueden cancelar citas finalizadas.
    /// Postcondición: Estado == Cancelada y se eleva CitaCanceladaEvent.
    /// </summary>
    public void Cancelar()
    {
        if (Estado == EstadoCita.Finalizada)
            throw new DomainException("No se puede cancelar una cita ya finalizada.");
        if (Estado == EstadoCita.Cancelada)
            throw new DomainException("La cita ya está cancelada.");

        Estado = EstadoCita.Cancelada;
        AddDomainEvent(new CitaCanceladaEvent(Id, Horario.FechaCompleta));
    }

    /// <summary>
    /// Finaliza la cita (fue atendida). Solo se pueden finalizar citas confirmadas.
    /// Postcondición: Estado == Finalizada y se eleva CitaFinalizadaEvent.
    /// </summary>
    public void Finalizar()
    {
        if (Estado != EstadoCita.Confirmada)
            throw new DomainException("Solo se pueden finalizar citas en estado Confirmada.");

        Estado = EstadoCita.Finalizada;
        AddDomainEvent(new CitaFinalizadaEvent(Id, Horario.FechaCompleta));
    }

    /// <summary>Invariante: la cita está activa (no cancelada ni finalizada).</summary>
    public bool EstaActiva() => Estado != EstadoCita.Cancelada && Estado != EstadoCita.Finalizada;

    // Constructor privado para EF Core
    private Cita() : base(Guid.Empty) { Horario = null!; }
}
