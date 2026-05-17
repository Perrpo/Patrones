namespace VetClinic.Facturacion.Domain.Events;
using VetClinic.BuildingBlocks.Domain;
public class PagoRegistradoEvent : IDomainEvent
{
    public Guid PagoId { get; } public Guid FacturaId { get; } public decimal Monto { get; }
    public DateTime OccurredOn { get; }
    public PagoRegistradoEvent(Guid pagoId, Guid facturaId, decimal monto)
    { PagoId = pagoId; FacturaId = facturaId; Monto = monto; OccurredOn = DateTime.UtcNow; }
}
