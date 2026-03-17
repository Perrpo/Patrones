namespace VetClinicConsole.Classes.Pagos;

using VetClinicConsole.Interfaces;

public class PagoTarjeta : IPago
{
    private bool _aprobado;

    public string Metodo => "Tarjeta";

    public bool Procesar(decimal monto)
    {
        _aprobado = monto > 0;
        return _aprobado;
    }

    public bool EstaAprobado() => _aprobado;
}
