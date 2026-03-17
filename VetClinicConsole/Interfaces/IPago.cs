namespace VetClinicConsole.Interfaces;

public interface IPago
{
    bool Procesar(decimal monto);
    bool EstaAprobado();
    string Metodo { get; }
}
