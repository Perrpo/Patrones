namespace VetClinic.Facturacion.Domain.Events;
using VetClinic.BuildingBlocks.Domain;
public class FacturaCreadaEvent : IDomainEvent
{
    public Guid FacturaId { get; } public Guid CitaId { get; }
    public DateTime OccurredOn { get; }
    public FacturaCreadaEvent(Guid facturaId, Guid citaId)
    { FacturaId = facturaId; CitaId = citaId; OccurredOn = DateTime.UtcNow; }
}
