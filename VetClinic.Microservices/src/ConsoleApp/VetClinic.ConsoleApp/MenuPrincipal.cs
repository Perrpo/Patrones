using VetClinic.Agendamiento.Application.DTOs;
using VetClinic.Agendamiento.Application.UseCases;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.Agendamiento.Domain.Services;
using VetClinic.Agendamiento.Domain.ValueObjects;
using VetClinic.Agendamiento.Domain.Enums;
using VetClinic.Facturacion.Application.DTOs;
using VetClinic.Facturacion.Application.UseCases;
using VetClinic.Facturacion.Domain.Aggregates;
using VetClinic.Facturacion.Domain.Interfaces;
using VetClinic.Notificaciones.Application.Consumers;
using VetClinic.Notificaciones.Application.UseCases;
using VetClinic.BuildingBlocks.Domain;

namespace VetClinic.ConsoleApp;

public class MenuPrincipal
{
    private readonly AgendarCitaUseCase _agendarCita;
    private readonly ConfirmarCitaUseCase _confirmarCita;
    private readonly CancelarCitaUseCase _cancelarCita;
    private readonly ConsultarAgendaUseCase _consultarAgenda;
    private readonly ConsultarDisponibilidadUseCase _consultarDisponibilidad;
    private readonly RegistrarPagoUseCase _registrarPago;
    private readonly ProcesarPendientesUseCase _procesarNotificaciones;
    private readonly CitaConfirmadaConsumer _citaConfirmadaConsumer;
    private readonly IAgendaRepository _agendaRepo;
    private readonly IFacturaRepository _facturaRepo;
    private readonly IMascotaRepository _mascotaRepo;
    private readonly ServicioDisponibilidad _servicioDisponibilidad;

    public MenuPrincipal(
        AgendarCitaUseCase agendarCita, ConfirmarCitaUseCase confirmarCita,
        CancelarCitaUseCase cancelarCita, ConsultarAgendaUseCase consultarAgenda,
        ConsultarDisponibilidadUseCase consultarDisponibilidad,
        RegistrarPagoUseCase registrarPago, ProcesarPendientesUseCase procesarNotificaciones,
        CitaConfirmadaConsumer citaConfirmadaConsumer,
        IAgendaRepository agendaRepo, IFacturaRepository facturaRepo,
        IMascotaRepository mascotaRepo, ServicioDisponibilidad servicioDisponibilidad)
    {
        _agendarCita = agendarCita; _confirmarCita = confirmarCita;
        _cancelarCita = cancelarCita; _consultarAgenda = consultarAgenda;
        _consultarDisponibilidad = consultarDisponibilidad;
        _registrarPago = registrarPago; _procesarNotificaciones = procesarNotificaciones;
        _citaConfirmadaConsumer = citaConfirmadaConsumer;
        _agendaRepo = agendaRepo; _facturaRepo = facturaRepo;
        _mascotaRepo = mascotaRepo; _servicioDisponibilidad = servicioDisponibilidad;
    }

    public async Task EjecutarAsync()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            MostrarEncabezado();
            Console.WriteLine("  1) Listar todo");
            Console.WriteLine("  2) Listar citas");
            Console.WriteLine("  3) Listar agendas");
            Console.WriteLine("  4) Crear mascota");
            Console.WriteLine("  5) Crear cita");
            Console.WriteLine("  6) Cambiar estado de una cita");
            Console.WriteLine("  7) Listar mascotas");
            Console.WriteLine("  8) Registrar pago");
            Console.WriteLine("  9) Buscar cita");
            Console.WriteLine("  10) Consultar disponibilidad");
            Console.WriteLine("  11) Procesar notificaciones");
            Console.WriteLine("  0) Salir");
            Console.Write("\n  Selecciona opcion: ");

            var opcion = Console.ReadLine()?.Trim();
            Console.WriteLine();

            try
            {
                switch (opcion)
                {
                    case "1": await ListarTodo(); break;
                    case "2": await ListarCitas(); break;
                    case "3": await ListarAgendas(); break;
                    case "4": await CrearMascota(); break;
                    case "5": await CrearCita(); break;
                    case "6": await CambiarEstadoCita(); break;
                    case "7": await ListarMascotas(); break;
                    case "8": await RegistrarPago(); break;
                    case "9": await BuscarCita(); break;
                    case "10": await ConsultarDisponibilidad(); break;
                    case "11": await ProcesarNotificaciones(); break;
                    case "0": Console.WriteLine("  Hasta luego!"); return;
                    default: MostrarError("Opcion invalida."); break;
                }
            }
            catch (DomainException ex) { MostrarError($"Error de dominio: {ex.Message}"); }
            catch (FormatException) { MostrarError("Formato de entrada invalido."); }
            catch (Exception ex) { MostrarError($"Error: {ex.Message}"); }

            Console.WriteLine("\n  Presione Enter para continuar...");
            Console.ReadLine();
        }
    }

    // ==================== 1) LISTAR TODO ====================
    private async Task ListarTodo()
    {
        Console.WriteLine("  === LISTADO GENERAL ===\n");
        await ListarMascotas();
        Console.WriteLine();
        await ListarAgendas();
        Console.WriteLine();
        await ListarCitas();
    }

    // ==================== 2) LISTAR CITAS ====================
    private async Task ListarCitas()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  === CITAS ===");
        Console.ResetColor();

        var agendas = await _agendaRepo.GetAllAsync();
        var todasCitas = agendas.SelectMany(a => a.Citas).ToList();

        if (todasCitas.Count == 0) { Console.WriteLine("  (Sin citas)"); return; }

        foreach (var c in todasCitas)
        {
            var agenda = agendas.First(a => a.Id == c.AgendaId);
            var color = c.Estado switch
            {
                EstadoCita.Confirmada => ConsoleColor.Green,
                EstadoCita.Cancelada => ConsoleColor.Red,
                EstadoCita.Finalizada => ConsoleColor.DarkGray,
                _ => ConsoleColor.White
            };
            Console.ForegroundColor = color;
            Console.Write($"  [{c.Estado,-11}] ");
            Console.ResetColor();
            var mascota = await _mascotaRepo.GetByIdAsync(c.MascotaId);
            var nombreMascota = mascota?.Nombre ?? "Desconocida";
            Console.WriteLine($"{c.Horario.Fecha:dd/MM/yyyy} {c.Horario.HoraInicio:hh\\:mm}-{c.Horario.HoraFin:hh\\:mm} | {nombreMascota,-8} | Prof: {agenda.NombreProfesional} | ID: {c.Id.ToString()[..8]}...");
        }
    }

    // ==================== 3) LISTAR AGENDAS ====================
    private async Task ListarAgendas()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  === AGENDAS ===");
        Console.ResetColor();

        var agendas = await _agendaRepo.GetAllAsync();
        foreach (var a in agendas)
        {
            Console.WriteLine($"  {a.NombreProfesional} | Citas: {a.Citas.Count} | Horarios laborales: {a.HorariosLaborales.Count}");
        }
    }

    // ==================== 4) CREAR MASCOTA ====================
    private async Task CrearMascota()
    {
        Console.WriteLine("  === Crear Mascota ===");

        Console.Write("  Nombre de la mascota: ");
        var nombre = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(nombre)) { MostrarError("Nombre obligatorio."); return; }

        Console.Write("  Especie: ");
        var especie = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(especie)) { MostrarError("Especie obligatoria."); return; }

        Console.Write("  Raza: ");
        var raza = (Console.ReadLine() ?? "").Trim();

        Console.Write("  Edad (anios): ");
        var edadTxt = (Console.ReadLine() ?? "").Trim();
        if (!int.TryParse(edadTxt, out var edad) || edad < 0)
        { MostrarError("Edad invalida. Debe ser un numero >= 0."); return; }

        var mascota = new Agendamiento.Domain.Entities.Mascota(
            Guid.NewGuid(), nombre, especie, raza, edad, DatosSemilla.ClienteJuanId);
        await _mascotaRepo.AddAsync(mascota);

        MostrarExito($"Mascota '{nombre}' creada. ID: {mascota.Id.ToString()[..8]}...");
    }

    // ==================== 5) CREAR CITA ====================
    private async Task CrearCita()
    {
        Console.WriteLine("  === Crear Cita ===");

        // 1. Elegir mascota
        var mascota = await ElegirMascota();
        if (mascota == null) return;

        // 2. Elegir profesional
        var (profesionalId, agenda) = await ElegirProfesional();
        if (agenda == null) return;

        // 3. Elegir servicio
        var servicioElegido = ElegirServicio();
        if (servicioElegido == null) return;

        // 4. Pedir fecha: dia, mes, anio
        var fecha = LeerFecha();
        if (fecha == null) return;

        // 5. Mostrar disponibilidad y elegir horario
        var duracion = new DuracionServicio(servicioElegido.Value.duracionMinutos);
        var slots = _servicioDisponibilidad.CalcularDisponibilidad(agenda, fecha.Value, duracion);

        if (slots.Count == 0)
        {
            MostrarError("No hay horarios disponibles para ese profesional en esa fecha.");
            return;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n  Horarios disponibles ({fecha.Value:dd/MM/yyyy}):");
        Console.ResetColor();
        for (int i = 0; i < slots.Count; i++)
        {
            var s = slots[i];
            Console.WriteLine($"  {i + 1}) {s.HoraInicio:hh\\:mm} - {s.HoraFin:hh\\:mm}");
        }

        Console.Write("  Seleccione horario (numero): ");
        var slotInput = Console.ReadLine()?.Trim();
        if (!int.TryParse(slotInput, out var slotIdx) || slotIdx < 1 || slotIdx > slots.Count)
        { MostrarError("Seleccion invalida."); return; }

        var slotElegido = slots[slotIdx - 1];

        // 6. Agendar via Use Case (pasa por Aggregate Root)
        var request = new CrearCitaRequest(
            profesionalId, mascota.Id, fecha.Value, slotElegido.HoraInicio, slotElegido.HoraFin);
        var resultado = await _agendarCita.EjecutarAsync(request);

        // 7. Crear factura automatica (evento CitaCreadaEvent -> BC Facturacion)
        var factura = new Factura(Guid.NewGuid(), resultado.Id, servicioElegido.Value.precio);
        await _facturaRepo.AddAsync(factura);
        factura.ClearDomainEvents();

        MostrarExito($"Cita creada exitosamente!");
        Console.WriteLine($"    Mascota: {mascota.Nombre}");
        Console.WriteLine($"    Profesional: {agenda.NombreProfesional}");
        Console.WriteLine($"    Servicio: {servicioElegido.Value.nombre}");
        Console.WriteLine($"    Fecha: {resultado.Fecha:dd/MM/yyyy} {resultado.HoraInicio}-{resultado.HoraFin}");
        Console.WriteLine($"    Estado: {resultado.Estado}");
        Console.WriteLine($"    Factura: ${servicioElegido.Value.precio:N0}");
    }

    // ==================== 6) CAMBIAR ESTADO DE CITA ====================
    private async Task CambiarEstadoCita()
    {
        Console.WriteLine("  === Cambiar Estado de Cita ===");

        var (cita, agenda) = await ElegirCita();
        if (cita == null || agenda == null) return;

        Console.WriteLine($"\n  Estado actual: {cita.Estado}");
        Console.WriteLine("  Opciones: confirmar | cancelar | finalizar");
        Console.Write("  Nuevo estado: ");
        var estado = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

        switch (estado)
        {
            case "confirmar":
                cita.Confirmar();
                // Evento -> BC Notificaciones
                var mascota = await _mascotaRepo.GetByIdAsync(cita.MascotaId);
                await _citaConfirmadaConsumer.HandleAsync(
                    cita.Id, "Cliente", "cliente@email.com", cita.Horario.FechaCompleta);
                break;
            case "cancelar":
                cita.Cancelar();
                break;
            case "finalizar":
                cita.Finalizar();
                break;
            default:
                MostrarError("Estado invalido.");
                return;
        }

        await _agendaRepo.UpdateAsync(agenda);
        MostrarExito($"Estado actualizado a: {cita.Estado}");
    }

    // ==================== 7) LISTAR MASCOTAS ====================
    private async Task ListarMascotas()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  === MASCOTAS ===");
        Console.ResetColor();

        var mascotas = await _mascotaRepo.GetByClienteIdAsync(DatosSemilla.ClienteJuanId);
        if (mascotas.Count == 0) { Console.WriteLine("  (Sin mascotas)"); return; }

        foreach (var m in mascotas)
            Console.WriteLine($"  {m.Id.ToString()[..8]}... | {m.Nombre,-10} | {m.Especie,-6} | {m.Raza,-18} | {m.Edad} anios");
    }

    // ==================== 8) REGISTRAR PAGO ====================
    private async Task RegistrarPago()
    {
        Console.WriteLine("  === Registrar Pago ===");

        var (cita, agenda) = await ElegirCita();
        if (cita == null) return;

        var factura = await _facturaRepo.GetByCitaIdAsync(cita.Id);
        if (factura == null) { MostrarError("No hay factura para esta cita."); return; }

        Console.WriteLine($"  Total: ${factura.Total.Cantidad:N0} | Pagada: {(factura.EstaPagada ? "Si" : "No")}");

        if (factura.EstaPagada)
        { MostrarError("La factura ya esta pagada."); return; }

        Console.Write("  Metodo (Efectivo/Tarjeta/Transferencia): ");
        var metodo = (Console.ReadLine() ?? "Efectivo").Trim();
        if (string.IsNullOrWhiteSpace(metodo)) metodo = "Efectivo";

        // Capitalizar primera letra
        metodo = char.ToUpper(metodo[0]) + metodo[1..].ToLower();

        var resultado = await _registrarPago.EjecutarAsync(
            factura.Id, new RegistrarPagoRequest(factura.Total.Cantidad, metodo), CancellationToken.None);

        MostrarExito($"Pago registrado: ${resultado.Monto:N0} ({resultado.Metodo}) - Estado: {resultado.Estado}");
    }

    // ==================== 9) BUSCAR CITA ====================
    private async Task BuscarCita()
    {
        Console.WriteLine("  === Buscar Cita ===");
        Console.Write("  Nombre de mascota: ");
        var nombre = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(nombre)) { Console.WriteLine("  Operacion cancelada."); return; }

        var lower = nombre.ToLowerInvariant();
        var agendas = await _agendaRepo.GetAllAsync();
        var todasCitas = agendas.SelectMany(a => a.Citas).ToList();

        var resultados = new List<(Agendamiento.Domain.Entities.Cita cita, string mascotaNombre, string profNombre)>();
        foreach (var c in todasCitas)
        {
            var mascota = await _mascotaRepo.GetByIdAsync(c.MascotaId);
            if (mascota != null && mascota.Nombre.ToLowerInvariant().Contains(lower))
            {
                var ag = agendas.First(a => a.Id == c.AgendaId);
                resultados.Add((c, mascota.Nombre, ag.NombreProfesional));
            }
        }

        if (resultados.Count == 0) { Console.WriteLine("  No se encontraron citas."); return; }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n  Se encontraron {resultados.Count} cita(s):");
        Console.ResetColor();

        foreach (var (c, mascotaN, profN) in resultados)
        {
            Console.WriteLine($"  Cita: {c.Id.ToString()[..8]}...");
            Console.WriteLine($"    Mascota: {mascotaN}");
            Console.WriteLine($"    Profesional: {profN}");
            Console.WriteLine($"    Fecha: {c.Horario.Fecha:dd/MM/yyyy} {c.Horario.HoraInicio:hh\\:mm}-{c.Horario.HoraFin:hh\\:mm}");
            Console.WriteLine($"    Estado: {c.Estado}");

            var factura = await _facturaRepo.GetByCitaIdAsync(c.Id);
            if (factura != null)
                Console.WriteLine($"    Factura: ${factura.Total.Cantidad:N0} | Pagada: {(factura.EstaPagada ? "Si" : "No")}");
            Console.WriteLine();
        }
    }

    // ==================== 10) CONSULTAR DISPONIBILIDAD ====================
    private async Task ConsultarDisponibilidad()
    {
        Console.WriteLine("  === Consultar Disponibilidad ===");
        var (profesionalId, agenda) = await ElegirProfesional();
        if (agenda == null) return;

        var fecha = LeerFecha();
        if (fecha == null) return;

        var duracion = new DuracionServicio(30);
        var slots = _servicioDisponibilidad.CalcularDisponibilidad(agenda, fecha.Value, duracion);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n  Disponibilidad de {agenda.NombreProfesional} - {fecha.Value:dd/MM/yyyy} (slots de 30 min):");
        Console.ResetColor();

        if (slots.Count == 0)
        { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("  Sin disponibilidad"); Console.ResetColor(); return; }

        for (int i = 0; i < slots.Count; i++)
        {
            var s = slots[i];
            Console.WriteLine($"  {i + 1}) {s.HoraInicio:hh\\:mm} - {s.HoraFin:hh\\:mm}");
        }
    }

    // ==================== 11) PROCESAR NOTIFICACIONES ====================
    private async Task ProcesarNotificaciones()
    {
        Console.WriteLine("  === Procesar Notificaciones ===");
        var enviadas = await _procesarNotificaciones.EjecutarAsync();
        if (enviadas > 0) MostrarExito($"{enviadas} notificacion(es) enviada(s).");
        else Console.WriteLine("  No hay notificaciones pendientes.");
    }

    // ==================== HELPERS ====================

    private DateTime? LeerFecha()
    {
        Console.Write("  Fecha (DD/MM/AAAA): ");
        var input = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(input))
        { MostrarError("Fecha obligatoria."); return null; }

        if (!DateTime.TryParseExact(input, "d/M/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var fecha))
        { MostrarError("Formato invalido. Use DD/MM/AAAA (ej: 18/05/2026)."); return null; }

        if (fecha.Date < DateTime.Today)
        { MostrarError("La fecha no puede ser anterior a hoy."); return null; }

        return fecha;
    }

    private async Task<Agendamiento.Domain.Entities.Mascota?> ElegirMascota()
    {
        Console.WriteLine("\n  Mascotas disponibles:");
        var mascotas = await _mascotaRepo.GetByClienteIdAsync(DatosSemilla.ClienteJuanId);
        if (mascotas.Count == 0) { MostrarError("No hay mascotas registradas."); return null; }

        for (int i = 0; i < mascotas.Count; i++)
            Console.WriteLine($"  {i + 1}) {mascotas[i].Nombre} - {mascotas[i].Especie} {mascotas[i].Raza}");

        Console.Write("  Seleccione (numero): ");
        var input = Console.ReadLine()?.Trim();
        if (!int.TryParse(input, out var idx) || idx < 1 || idx > mascotas.Count)
        { MostrarError("Seleccion invalida."); return null; }

        return mascotas[idx - 1];
    }

    private async Task<(Guid profesionalId, Agendamiento.Domain.Aggregates.Agenda? agenda)> ElegirProfesional()
    {
        Console.WriteLine("\n  Profesionales:");
        var agendas = await _agendaRepo.GetAllAsync();
        for (int i = 0; i < agendas.Count; i++)
            Console.WriteLine($"  {i + 1}) {agendas[i].NombreProfesional}");

        Console.Write("  Seleccione (numero): ");
        var input = Console.ReadLine()?.Trim();
        if (!int.TryParse(input, out var idx) || idx < 1 || idx > agendas.Count)
        { MostrarError("Seleccion invalida."); return (Guid.Empty, null); }

        var agenda = agendas[idx - 1];
        return (agenda.ProfesionalId, agenda);
    }

    private async Task<(Agendamiento.Domain.Entities.Cita? cita, Agendamiento.Domain.Aggregates.Agenda? agenda)> ElegirCita()
    {
        Console.WriteLine("\n  Citas disponibles:");
        var agendas = await _agendaRepo.GetAllAsync();
        var listado = new List<(Agendamiento.Domain.Entities.Cita cita, Agendamiento.Domain.Aggregates.Agenda agenda, string mascotaNombre)>();

        foreach (var a in agendas)
        {
            foreach (var c in a.Citas)
            {
                var mascota = await _mascotaRepo.GetByIdAsync(c.MascotaId);
                listado.Add((c, a, mascota?.Nombre ?? "Desconocida"));
            }
        }

        if (listado.Count == 0) { Console.WriteLine("  (Sin citas)"); return (null, null); }

        for (int i = 0; i < listado.Count; i++)
        {
            var (c, a, mn) = listado[i];
            var color = c.Estado switch
            {
                EstadoCita.Confirmada => ConsoleColor.Green,
                EstadoCita.Cancelada => ConsoleColor.Red,
                EstadoCita.Finalizada => ConsoleColor.DarkGray,
                _ => ConsoleColor.White
            };
            Console.ForegroundColor = color;
            Console.Write($"  {i + 1}) [{c.Estado,-11}] ");
            Console.ResetColor();
            Console.WriteLine($"{c.Horario.Fecha:dd/MM/yyyy} {c.Horario.HoraInicio:hh\\:mm}-{c.Horario.HoraFin:hh\\:mm} | {mn} | {a.NombreProfesional}");
        }

        Console.Write("  Seleccione (numero) o vacio para cancelar: ");
        var input = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(input)) { Console.WriteLine("  Operacion cancelada."); return (null, null); }
        if (!int.TryParse(input, out var idx) || idx < 1 || idx > listado.Count)
        { MostrarError("Seleccion invalida."); return (null, null); }

        return (listado[idx - 1].cita, listado[idx - 1].agenda);
    }

    private (string nombre, int duracionMinutos, decimal precio)? ElegirServicio()
    {
        Console.WriteLine("\n  Servicios disponibles:");
        var servicios = new[]
        {
            (nombre: "Consulta Veterinaria", duracionMinutos: 30, precio: 50000m),
            (nombre: "Vacunacion", duracionMinutos: 15, precio: 30000m),
            (nombre: "Corte de Pelo y Banio", duracionMinutos: 60, precio: 40000m),
            (nombre: "Cirugia Menor", duracionMinutos: 120, precio: 250000m)
        };

        for (int i = 0; i < servicios.Length; i++)
        {
            var s = servicios[i];
            Console.WriteLine($"  {i + 1}) {s.nombre} ({s.duracionMinutos} min) - ${s.precio:N0}");
        }

        Console.Write("  Seleccione (numero): ");
        var input = Console.ReadLine()?.Trim();
        if (!int.TryParse(input, out var idx) || idx < 1 || idx > servicios.Length)
        { MostrarError("Seleccion invalida."); return null; }

        return servicios[idx - 1];
    }

    private void MostrarEncabezado()
    {
        try { Console.Clear(); } catch { Console.WriteLine("\n\n"); }
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("  ================================================");
        Console.WriteLine("    CLINICA VETERINARIA - Microservicios DDD");
        Console.WriteLine("    Agendamiento | Facturacion | Notificaciones");
        Console.WriteLine("  ================================================");
        Console.ResetColor();
        Console.WriteLine();
    }

    private void MostrarExito(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n  OK: {msg}");
        Console.ResetColor();
    }

    private void MostrarError(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n  ERROR: {msg}");
        Console.ResetColor();
    }
}
