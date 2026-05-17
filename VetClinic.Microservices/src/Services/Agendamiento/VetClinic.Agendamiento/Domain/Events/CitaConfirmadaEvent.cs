namespace VetClinic.Agendamiento.Domain.Events;

using VetClinic.BuildingBlocks.Domain;

/// <summary>Evento: una cita fue confirmada por el profesional.</summary>
public class CitaConfirmadaEvent : IDomainEvent
{
    public Guid CitaId { get; }
    public DateTime FechaCita { get; }
    public DateTime OccurredOn { get; }

    public CitaConfirmadaEvent(Guid citaId, DateTime fechaCita)
    {
        CitaId = citaId;
        FechaCita = fechaCita;
        OccurredOn = DateTime.UtcNow;
    }
}
