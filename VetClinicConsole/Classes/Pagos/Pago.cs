namespace VetClinicConsole.Classes.Pagos;

using VetClinicConsole.Interfaces;

public class Pago
{
    public int Id { get; }
    public decimal Monto { get; }
    public DateTime Fecha { get; }
    public EstadoPago Estado { get; private set; }
    public IPago Metodo { get; }

    public Pago(int id, decimal monto, IPago metodo)
    {
        Id = id;
        Monto = monto;
        Metodo = metodo;
        Fecha = DateTime.Now;
        Estado = EstadoPago.Pendiente;
    }

    public bool Procesar()
    {
        var aprobado = Metodo.Procesar(Monto);
        Estado = aprobado ? EstadoPago.Pagado : EstadoPago.Pendiente;
        return aprobado;
    }

    public bool EstaAprobado() => Estado == EstadoPago.Pagado && Metodo.EstaAprobado();
}
