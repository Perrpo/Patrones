namespace VetClinicConsole.Classes.Pagos;

using VetClinicConsole.Interfaces;

public class PagoEfectivo : IPago
{
    private bool _aprobado;

    public string Metodo => "Efectivo";

    public bool Procesar(decimal monto)
    {
        _aprobado = monto >= 0;
        return _aprobado;
    }

    public bool EstaAprobado() => _aprobado;
}
