using VetClinicConsole.Classes.Agendas;
using VetClinicConsole.Classes.Citas;
using VetClinicConsole.Classes.Pagos;
using VetClinicConsole.Classes.Personal;
using VetClinicConsole.Interfaces;
using VetClinicConsole.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

var data = InMemoryData.Load();
var reporter = new DataReporter(data);

while (true)
{
Console.WriteLine("=== Veterinaria - Gestión de Citas ===");
    Console.WriteLine("1) Listar todo");
    Console.WriteLine("2) Listar citas");
    Console.WriteLine("3) Listar agendas");
    Console.WriteLine("4) Crear cliente");
    Console.WriteLine("5) Crear mascota");
    Console.WriteLine("6) Crear cita");
    Console.WriteLine("7) Cambiar estado de una cita");
    Console.WriteLine("8) Listar mascotas");
    Console.WriteLine("9) Registrar pago");
    Console.WriteLine("10) Buscar cita");
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
                reporter.PrintAgendas();
                break;
            case "4":
                CrearCliente();
                break;
            case "5":
                CrearMascota();
                break;
            case "6":
                CrearCita();
                break;
            case "7":
                CambiarEstadoCita();
                break;
            case "8":
                reporter.PrintMascotas();
                break;
            case "9":
                RegistrarPago();
                break;
            case "10":
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
    var mascota = BuscarMascotaPorNombre();
    if (mascota is null)
    {
        Console.WriteLine("Mascota no encontrada.\n");
        return;
    }

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

    var slot = ElegirDeLista(disponibilidad, h =>
    {
        var inicio = h.Fecha + h.HoraInicio;
        var fin = h.Fecha + h.HoraFin;
        return $"{inicio:dd/MM/yyyy hh:mm tt} - {fin:hh:mm tt}";
    });
    if (slot is null) return;

    var mensaje = data.Facade.AgendarCita(slot.Fecha + slot.HoraInicio, mascota!, profesional!, new[] { servicio! });
    Console.WriteLine($"{mensaje}\n");
}

void CrearCliente()
{
    Console.WriteLine("=== Crear Cliente ===");
    Console.Write("Nombre completo: ");
    var nombre = (Console.ReadLine() ?? "").Trim();
    if (string.IsNullOrWhiteSpace(nombre))
    {
        Console.WriteLine("Nombre obligatorio.\n");
        return;
    }

    Console.Write("Email: ");
    var email = (Console.ReadLine() ?? "").Trim();
    if (string.IsNullOrWhiteSpace(email))
    {
        Console.WriteLine("Email obligatorio.\n");
        return;
    }

    var nuevoId = data.Clientes.Any() ? data.Clientes.Max(c => c.Id) + 1 : 1;
    var cliente = new Cliente(nuevoId, nombre, email);
    data.Clientes.Add(cliente);
    Console.WriteLine($"Cliente {cliente.Id} creado.\n");
}

void CrearMascota()
{
    Console.WriteLine("=== Crear Mascota ===");
    var cliente = ElegirCliente();
    if (cliente is null) return;

    Console.Write("Nombre de la mascota: ");
    var nombre = (Console.ReadLine() ?? "").Trim();
    if (string.IsNullOrWhiteSpace(nombre))
    {
        Console.WriteLine("Nombre obligatorio.\n");
        return;
    }

    Console.Write("Especie: ");
    var especie = (Console.ReadLine() ?? "").Trim();
    if (string.IsNullOrWhiteSpace(especie))
    {
        Console.WriteLine("Especie obligatoria.\n");
        return;
    }

    Console.Write("Raza: ");
    var raza = (Console.ReadLine() ?? "").Trim();

    Console.Write("Edad (años): ");
    var edadTxt = (Console.ReadLine() ?? "").Trim();
    if (!int.TryParse(edadTxt, out var edad) || edad < 0)
    {
        Console.WriteLine("Edad inválida.\n");
        return;
    }

    var nuevoId = data.Mascotas.Any() ? data.Mascotas.Max(m => m.Id) + 1 : 1;
    var mascota = new Mascota(nuevoId, nombre, especie, raza, edad, cliente);
    data.Mascotas.Add(mascota);
    Console.WriteLine($"Mascota {mascota.Id} creada para {cliente.Nombre}.\n");
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
    Console.Write("Nombre de mascota: ");
    var nombre = (Console.ReadLine() ?? "").Trim();
    if (string.IsNullOrWhiteSpace(nombre))
    {
        Console.WriteLine("Operación cancelada.\n");
        return;
    }

    var lower = nombre.ToLowerInvariant();
    var coincidencias = data.Citas
        .Where(c => c.Mascota.Nombre.ToLowerInvariant().Contains(lower))
        .ToList();

    if (coincidencias.Count == 0)
    {
        Console.WriteLine("No se encontraron citas.\n");
        return;
    }

    if (coincidencias.Count > 1)
    {
        Console.WriteLine("Se encontraron varias, elige una:");
        var seleccion = ElegirDeLista(coincidencias, c =>
        {
            var serviciosTxt = string.Join(", ", c.Factura.Servicios.Select(s => s.Nombre));
            return $"{c.Id} | {c.Mascota.Nombre} | {c.Horario.Fecha:yyyy-MM-dd} {c.Horario.HoraInicio} | {c.EstadoNombre} | {serviciosTxt}";
        });
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
    var servicios = string.Join(", ", c.Factura.Servicios.Select(s => $"{s.Nombre} ({s.Duracion()} min)"));
    Console.WriteLine($"Cita {c.Id}");
    Console.WriteLine($"Mascota: {c.Mascota.Nombre}");
    Console.WriteLine($"Profesional: {profNombre}");
    var inicio = c.Horario.Fecha + c.Horario.HoraInicio;
    Console.WriteLine($"Fecha: {inicio:yyyy-MM-dd hh:mm tt}");
    Console.WriteLine($"Servicios: {servicios}");
    Console.WriteLine($"Estado: {c.EstadoNombre}");
    Console.WriteLine($"Total: ${c.Factura.CalcularTotal():N0}");
    Console.WriteLine();
}

Mascota? BuscarMascotaPorNombre()
{
    Console.Write("Nombre de la mascota: ");
    var nombre = (Console.ReadLine() ?? "").Trim();
    if (string.IsNullOrWhiteSpace(nombre))
    {
        Console.WriteLine("Operación cancelada.\n");
        return null;
    }

    var lower = nombre.ToLowerInvariant();
    var coincidencias = data.Mascotas.Where(m => m.Nombre.ToLowerInvariant().Contains(lower)).ToList();
    if (coincidencias.Count == 0) return null;
    if (coincidencias.Count == 1) return coincidencias[0];

    Console.WriteLine("Se encontraron varias mascotas, elige una:");
    return ElegirDeLista(coincidencias, m => $"{m.Nombre} ({m.Especie}) - Dueño: {m.Cliente.Nombre}");
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

Cliente? ElegirCliente()
{
    Console.WriteLine("Elige cliente:");
    return ElegirDeLista(data.Clientes, c => $"{c.Id}) {c.Nombre} | {c.Email}");
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
        var inicio = c.Horario.Fecha + c.Horario.HoraInicio;
        var serviciosTxt = string.Join(", ", c.Factura.Servicios.Select(s => s.Nombre));
        return $"{c.Id} | {c.Mascota.Nombre} | {prof} | {inicio:yyyy-MM-dd hh:mm tt} | {c.EstadoNombre} | {serviciosTxt}";
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
        if (!DateTime.TryParse(txt, out var value))
        {
            Console.WriteLine("Formato de fecha/hora inválido.");
            continue;
        }

        var soloFecha = value.Date;
        if (soloFecha < DateTime.Today)
        {
            Console.WriteLine("La fecha no puede ser anterior a hoy.");
            continue;
        }

        return soloFecha;
    }
}
