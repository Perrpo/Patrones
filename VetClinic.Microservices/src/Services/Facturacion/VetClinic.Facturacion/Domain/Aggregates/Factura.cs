namespace VetClinic.Facturacion.Domain.Aggregates;
using VetClinic.BuildingBlocks.Domain;
using VetClinic.Facturacion.Domain.Entities;
using VetClinic.Facturacion.Domain.Events;
using VetClinic.Facturacion.Domain.ValueObjects;
/// <summary>AGGREGATE ROOT de Facturacion. Unico punto de acceso para pagos.</summary>
public class Factura : AggregateRoot<Guid>
{
    public Guid CitaId { get; private set; }
    public Dinero Total { get; private set; }
    private readonly List<Pago> _pagos = new();
    public IReadOnlyList<Pago> Pagos => _pagos.AsReadOnly();
    public bool EstaPagada => _pagos.Where(p => p.EstaAprobado()).Sum(p => p.Monto.Cantidad) >= Total.Cantidad;

    public Factura(Guid id, Guid citaId, decimal total) : base(id)
    {
        CitaId = citaId;
        Total = new Dinero(total);
        AddDomainEvent(new FacturaCreadaEvent(id, citaId));
    }

    /// <summary>Registra un pago. Solo a traves de la raiz del agregado.</summary>
    public Pago RegistrarPago(decimal monto, string metodoPago)
    {
        if (EstaPagada) throw new DomainException("La factura ya esta pagada.");
        var pago = new Pago(Guid.NewGuid(), new Dinero(monto), new MetodoPago(metodoPago));
        pago.Aprobar();
        _pagos.Add(pago);
        AddDomainEvent(new PagoRegistradoEvent(pago.Id, Id, monto));
        return pago;
    }

    private Factura() : base(Guid.Empty) { Total = null!; }
}
