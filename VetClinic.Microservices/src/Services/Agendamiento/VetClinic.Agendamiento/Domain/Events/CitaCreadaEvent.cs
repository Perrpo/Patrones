namespace VetClinic.Agendamiento.Domain.Events;

using VetClinic.BuildingBlocks.Domain;

/// <summary>Evento: una cita fue creada en el sistema.</summary>
public class CitaCreadaEvent : IDomainEvent
{
    public Guid CitaId { get; }
    public Guid MascotaId { get; }
    public Guid ProfesionalId { get; }
    public DateTime FechaCita { get; }
    public DateTime OccurredOn { get; }

    public CitaCreadaEvent(Guid citaId, Guid mascotaId, Guid profesionalId, DateTime fechaCita)
    {
        CitaId = citaId;
        MascotaId = mascotaId;
        ProfesionalId = profesionalId;
        FechaCita = fechaCita;
        OccurredOn = DateTime.UtcNow;
    }
}
