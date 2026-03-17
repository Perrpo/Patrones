namespace VetClinicConsole.Classes.Facturacion;

using VetClinicConsole.Classes.Pagos;
using VetClinicConsole.Interfaces;

public class Factura
{
    public int Id { get; }
    public List<IServicio> Servicios { get; } = new();
    public List<Pago> Pagos { get; } = new();

    public Factura(int id, object _)
    {
        Id = id;
    }

    public void AgregarServicio(IServicio servicio) => Servicios.Add(servicio);

    public void AgregarServicios(IEnumerable<IServicio> servicios)
    {
        Servicios.AddRange(servicios);
    }

    public decimal CalcularTotal() => Servicios.Sum(s => s.CalcularPrecio());

    public void RegistrarPago(Pago pago) => Pagos.Add(pago);

    public bool EstaPagada() =>
        Pagos.Any(p => p.EstaAprobado()) &&
        Pagos.Where(p => p.EstaAprobado()).Sum(p => p.Monto) >= CalcularTotal();
}
