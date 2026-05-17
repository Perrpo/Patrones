namespace VetClinic.Facturacion.Domain.Entities;
using VetClinic.BuildingBlocks.Domain;
using VetClinic.Facturacion.Domain.Enums;
using VetClinic.Facturacion.Domain.ValueObjects;
/// <summary>Entidad enriquecida: Pago con comportamiento de negocio.</summary>
public class Pago : Entity<Guid>
{
    public Dinero Monto { get; private set; }
    public MetodoPago Metodo { get; private set; }
    public DateTime Fecha { get; private set; }
    public EstadoPago Estado { get; private set; }

    public Pago(Guid id, Dinero monto, MetodoPago metodo) : base(id)
    {
        if (monto is null) throw new DomainException("El monto es obligatorio.");
        Monto = monto; Metodo = metodo; Fecha = DateTime.UtcNow; Estado = EstadoPago.Pendiente;
    }

    public void Aprobar()
    {
        if (Estado != EstadoPago.Pendiente) throw new DomainException("Solo se pueden aprobar pagos pendientes.");
        Estado = EstadoPago.Pagado;
    }

    public void Rechazar()
    {
        if (Estado != EstadoPago.Pendiente) throw new DomainException("Solo se pueden rechazar pagos pendientes.");
        Estado = EstadoPago.Rechazado;
    }

    public bool EstaAprobado() => Estado == EstadoPago.Pagado;
    private Pago() : base(Guid.Empty) { Monto = null!; Metodo = null!; }
}
