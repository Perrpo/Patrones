namespace VetClinicConsole.Classes.Servicios;

using VetClinicConsole.Interfaces;

public abstract class ServicioBase : IServicio
{
    public string Nombre { get; }
    public string EspecialidadRequerida { get; }
    private readonly decimal _precio;
    private readonly int _duracion;

    protected ServicioBase(string nombre, decimal precio, int duracion, string especialidadRequerida)
    {
        Nombre = nombre;
        _precio = precio;
        _duracion = duracion;
        EspecialidadRequerida = especialidadRequerida;
    }

    public decimal CalcularPrecio() => _precio;

    public int Duracion() => _duracion;
}
