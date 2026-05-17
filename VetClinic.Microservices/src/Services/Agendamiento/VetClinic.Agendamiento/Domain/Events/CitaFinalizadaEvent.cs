namespace VetClinic.Agendamiento.Domain.Events;

using VetClinic.BuildingBlocks.Domain;

/// <summary>Evento: una cita fue finalizada (atendida).</summary>
public class CitaFinalizadaEvent : IDomainEvent
{
    public Guid CitaId { get; }
    public DateTime FechaCita { get; }
    public DateTime OccurredOn { get; }

    public CitaFinalizadaEvent(Guid citaId, DateTime fechaCita)
    {
        CitaId = citaId;
        FechaCita = fechaCita;
        OccurredOn = DateTime.UtcNow;
    }
}
