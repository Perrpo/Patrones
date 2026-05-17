namespace VetClinic.Agendamiento.Domain.Events;

using VetClinic.BuildingBlocks.Domain;

/// <summary>Evento: una cita fue cancelada.</summary>
public class CitaCanceladaEvent : IDomainEvent
{
    public Guid CitaId { get; }
    public DateTime FechaCita { get; }
    public DateTime OccurredOn { get; }

    public CitaCanceladaEvent(Guid citaId, DateTime fechaCita)
    {
        CitaId = citaId;
        FechaCita = fechaCita;
        OccurredOn = DateTime.UtcNow;
    }
}
