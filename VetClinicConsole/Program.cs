using VetClinicConsole.Classes.Agendas;
using VetClinicConsole.Classes.Citas;
using VetClinicConsole.Classes.Pagos;
using VetClinicConsole.Classes.Personal;
using VetClinicConsole.Interfaces;
using VetClinicConsole.Services;

var data = InMemoryData.Load();
var reporter = new DataReporter(data);

while (true)
{
    Console.WriteLine("=== Veterinaria - Gestión de Citas ===");
    Console.WriteLine("1) Listar todo");
    Console.WriteLine("2) Listar citas");
    Console.WriteLine("3) Crear cita");
    Console.WriteLine("4) Cambiar estado de una cita");
    Console.WriteLine("5) Listar mascotas");
    Console.WriteLine("6) Registrar pago");
    Console.WriteLine("7) Buscar cita");
    Console.WriteLine("0) Salir");
    Console.Write("Selecciona opción: ");

    var option = Console.ReadLine();
    Console.WriteLine();

    try
    {
        switch (option)
        {
            case "1":
                reporter.PrintEverything();
                break;
            case "2":
                reporter.PrintCitas();
                break;
            case "3":
                CrearCita();
                break;
            case "4":
                CambiarEstadoCita();
                break;
            case "5":
                reporter.PrintMascotas();
                break;
            case "6":
                RegistrarPago();
                break;
            case "7":
                BuscarCita();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Opción inválida.\n");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}\n");
    }
}

void CrearCita()
{
    Console.WriteLine("=== Crear Cita ===");
    var mascota = ElegirMascota();
    if (mascota is null) return;

    var servicio = ElegirServicio();
    if (servicio is null) return;

    var profesional = ElegirProfesional(servicio!);
    if (profesional is null) return;

    var fecha = ReadDate("Fecha (yyyy-MM-dd): ").Date;

    var duracion = TimeSpan.FromMinutes(servicio!.Duracion());
    var disponibilidad = profesional.VerDisponibilidad(fecha, duracion);
    if (disponibilidad.Count == 0)
    {
        Console.WriteLine("No hay horarios disponibles para ese profesional en esa fecha.\n");
        return;
    }

    var slot = ElegirDeLista(disponibilidad, h => $"{h.Fecha:dd/MM/yyyy} {h.HoraInicio:hh\\:mm} - {h.HoraFin:hh\\:mm}");
    if (slot is null) return;

    var mensaje = data.Facade.AgendarCita(slot.Fecha + slot.HoraInicio, mascota!, profesional!, new[] { servicio! });
    Console.WriteLine($"{mensaje}\n");
}

void CambiarEstadoCita()
{
    Console.WriteLine("=== Cambiar Estado de Cita ===");
    var cita = ElegirCita();
    if (cita is null) return;

    Console.WriteLine("Estados válidos: confirmar | cancelar | finalizar");
    Console.Write("Nuevo estado: ");
    var estado = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

    switch (estado)
    {
        case "confirmar":
            cita!.Confirmar();
            break;
        case "cancelar":
            cita!.Cancelar();
            break;
        case "finalizar":
            cita!.Finalizar();
            break;
        default:
            Console.WriteLine("Estado inválido.\n");
            return;
    }

    Console.WriteLine("Estado actualizado.\n");
}

void RegistrarPago()
{
    Console.WriteLine("=== Registrar Pago ===");
    var cita = ElegirCita();
    if (cita is null) return;

    var monto = cita!.Factura.CalcularTotal();
    Console.WriteLine($"Total de la cita: ${monto:N0}");
    Console.Write("Método (tarjeta/efectivo/transferencia): ");
    var metodoTxt = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

    IPago metodo = metodoTxt switch
    {
        "tarjeta" => new PagoTarjeta(),
        "efectivo" => new PagoEfectivo(),
        "transferencia" => new PagoTransferencia(),
        _ => new PagoEfectivo()
    };

    var pago = new Pago(data.Pagos.Count + 1, monto, metodo);
    pago.Procesar();
    cita.Factura.RegistrarPago(pago);
    data.Pagos.Add(pago);

    Console.WriteLine($"Pago {pago.Id} registrado. Estado: {pago.Estado}\n");
}

void BuscarCita()
{
    Console.WriteLine("=== Buscar Cita ===");
    Console.Write("ID o nombre de mascota: ");
    var filtro = (Console.ReadLine() ?? "").Trim();
    if (string.IsNullOrWhiteSpace(filtro))
    {
        Console.WriteLine("Operación cancelada.\n");
        return;
    }

    List<Cita> coincidencias;
    if (int.TryParse(filtro, out var id))
    {
        coincidencias = data.Citas.Where(c => c.Id == id).ToList();
    }
    else
    {
        var lower = filtro.ToLowerInvariant();
        coincidencias = data.Citas
            .Where(c => c.Mascota.Nombre.ToLowerInvariant().Contains(lower))
            .ToList();
    }

    if (coincidencias.Count == 0)
    {
        Console.WriteLine("No se encontraron citas.\n");
        return;
    }

    if (coincidencias.Count > 1)
    {
        Console.WriteLine("Se encontraron varias, elige una:");
        var seleccion = ElegirDeLista(coincidencias, c => $"{c.Id} | {c.Mascota.Nombre} | {c.Horario.Fecha:yyyy-MM-dd} {c.Horario.HoraInicio} | {c.EstadoNombre}");
        if (seleccion is null) return;
        ImprimirCitaDetallada(seleccion);
    }
    else
    {
        ImprimirCitaDetallada(coincidencias[0]);
    }
}

void ImprimirCitaDetallada(Cita c)
{
    var profNombre = c.Profesional switch
    {
        Veterinario v => v.Nombre,
        Peluquero p => p.Nombre,
        Asistente a => a.Nombre,
        _ => c.Profesional.GetType().Name
    };
    Console.WriteLine($"Cita {c.Id}");
    Console.WriteLine($"Mascota: {c.Mascota.Nombre}");
    Console.WriteLine($"Profesional: {profNombre}");
    Console.WriteLine($"Fecha: {c.Horario.Fecha:yyyy-MM-dd} {c.Horario.HoraInicio}");
    Console.WriteLine($"Estado: {c.EstadoNombre}");
    Console.WriteLine($"Total: ${c.Factura.CalcularTotal():N0}");
    Console.WriteLine();
}

Mascota? ElegirMascota()
{
    Console.WriteLine("Elige mascota:");
    return ElegirDeLista(data.Mascotas, m => $"{m.Id}) {m.Nombre} ({m.Especie}) - Dueño: {m.Cliente.Nombre}");
}

IAgendable? ElegirProfesional(IServicio servicio)
{
    Console.WriteLine("Elige profesional:");
    var candidatos = data.Profesionales
        .Where(p => p.PuedeRealizar(servicio))
        .ToList();
    if (!candidatos.Any())
    {
        Console.WriteLine("No hay profesionales disponibles para este servicio.\n");
        return null;
    }
    return ElegirDeLista(candidatos, p => p switch
    {
        Veterinario v => $"{v.Nombre} - Veterinario Esp: {v.Especialidad}",
        Peluquero pe => $"{pe.Nombre} - Peluquero",
        Asistente a => $"{a.Nombre} - Asistente Dep: {a.Departamento}",
        _ => p.GetType().Name
    });
}

VetClinicConsole.Interfaces.IServicio? ElegirServicio()
{
    Console.WriteLine("Elige servicio:");
    return ElegirDeLista(data.Servicios, s => $"{s.Nombre} | ${s.CalcularPrecio():N0} | {s.Duracion()} min");
}

Cita? ElegirCita()
{
    Console.WriteLine("Elige cita:");
    return ElegirDeLista(data.Citas, c =>
    {
        var prof = c.Profesional switch
        {
            Veterinario v => v.Nombre,
            Peluquero p => p.Nombre,
            Asistente a => a.Nombre,
            _ => "Profesional"
        };
        return $"{c.Id} | {c.Mascota.Nombre} | {prof} | {c.Horario.Fecha:yyyy-MM-dd} {c.Horario.HoraInicio} | {c.EstadoNombre}";
    });
}

T? ElegirDeLista<T>(IReadOnlyList<T> items, Func<T, string> label)
{
    for (int i = 0; i < items.Count; i++)
    {
        Console.WriteLine($"{i + 1}) {label(items[i])}");
    }
    Console.Write("Opción (número) o vacío para cancelar: ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Operación cancelada.\n");
        return default;
    }
    if (int.TryParse(input, out var idx) && idx >= 1 && idx <= items.Count)
    {
        Console.WriteLine();
        return items[idx - 1];
    }
    Console.WriteLine("Opción inválida.\n");
    return default;
}

DateTime ReadDate(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var txt = Console.ReadLine();
        if (DateTime.TryParse(txt, out var value))
            return value;
        Console.WriteLine("Formato de fecha/hora inválido.");
    }
}
