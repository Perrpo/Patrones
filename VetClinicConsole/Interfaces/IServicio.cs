namespace VetClinicConsole.Interfaces;

public interface IServicio
{
    string Nombre { get; }
    string EspecialidadRequerida { get; }
    decimal CalcularPrecio();
    int Duracion(); // minutos
}
