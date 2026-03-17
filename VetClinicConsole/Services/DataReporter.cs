namespace VetClinicConsole.Services;

using System.Globalization;
using VetClinicConsole.Classes.Citas;
using VetClinicConsole.Classes.Pagos;
using VetClinicConsole.Interfaces;

public class DataReporter
{
    private readonly InMemoryData _data;
    private static readonly CultureInfo _culture = new("es-CO");

    public DataReporter(InMemoryData data)
    {
        _data = data;
    }

    public void PrintEverything()
    {
        PrintClientes();
        PrintMascotas();
        PrintProfesionales();
        PrintServicios();
        PrintCitas();
        PrintPagos();
    }

    public void PrintClientes()
    {
        Console.WriteLine("=== Clientes ===");
        foreach (var c in _data.Clientes.OrderBy(c => c.Id))
        {
            Console.WriteLine($"{c.Id} | {c.Nombre} | {c.Email}");
        }
        Console.WriteLine();
    }

    public void PrintMascotas()
    {
        Console.WriteLine("=== Mascotas ===");
        foreach (var m in _data.Mascotas.OrderBy(m => m.Id))
        {
            Console.WriteLine($"{m.Id} | {m.Nombre} ({m.Especie}) - {m.Raza} | Edad: {m.Edad} | Dueño: {m.Cliente.Nombre}");
        }
        Console.WriteLine();
    }

    public void PrintProfesionales()
    {
        Console.WriteLine("=== Profesionales ===");
        foreach (var p in _data.Profesionales)
        {
            var agenda = p.ObtenerAgenda();
            var label = p switch
            {
                Classes.Personal.Veterinario v => $"Veterinario Esp: {v.Especialidad}",
                Classes.Personal.Peluquero => "Peluquero",
                Classes.Personal.Asistente a => $"Asistente Dep: {a.Departamento}",
                _ => p.GetType().Name
            };
            Console.WriteLine($"{label} | Agenda: {agenda.Citas.Count} citas");
        }
        Console.WriteLine();
    }

    public void PrintServicios()
    {
        Console.WriteLine("=== Servicios ===");
        foreach (var s in _data.Servicios)
        {
            Console.WriteLine($"{s.Nombre} | ${s.CalcularPrecio().ToString("N0", _culture)} | Duración: {s.Duracion()} min");
        }
        Console.WriteLine();
    }

    public void PrintCitas()
    {
        Console.WriteLine("=== Citas ===");
        foreach (var c in _data.Citas.OrderBy(c => c.Id))
        {
            var profNombre = c.Profesional switch
            {
                Classes.Personal.Veterinario v => v.Nombre,
                Classes.Personal.Peluquero p => p.Nombre,
                Classes.Personal.Asistente a => a.Nombre,
                _ => c.Profesional.GetType().Name
            };
            Console.WriteLine($"{c.Id} | {c.Mascota.Nombre} | {profNombre} | {c.Horario.Fecha:d} {c.Horario.HoraInicio} | Estado: {c.EstadoNombre} | Total: ${c.Factura.CalcularTotal():N0}");
        }
        Console.WriteLine();
    }

    public void PrintPagos()
    {
        Console.WriteLine("=== Pagos ===");
        foreach (var p in _data.Pagos.OrderBy(p => p.Id))
        {
            Console.WriteLine($"{p.Id} | {p.Metodo.Metodo} | ${p.Monto.ToString("N0", _culture)} | {p.Fecha:g} | Estado: {p.Estado}");
        }
        Console.WriteLine();
    }
}
