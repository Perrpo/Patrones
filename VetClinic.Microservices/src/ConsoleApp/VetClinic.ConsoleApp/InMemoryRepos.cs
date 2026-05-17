using VetClinic.Agendamiento.Domain.Aggregates;
using VetClinic.Agendamiento.Domain.Entities;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.Facturacion.Domain.Aggregates;
using VetClinic.Facturacion.Domain.Interfaces;
using VetClinic.Notificaciones.Domain.Entities;
using VetClinic.Notificaciones.Domain.Enums;
using VetClinic.Notificaciones.Domain.Interfaces;
using VetClinic.BuildingBlocks.Application;
using VetClinic.BuildingBlocks.Domain;

namespace VetClinic.ConsoleApp.InMemoryRepos;

// ===== Repositorios en memoria (simulan BD aislada por microservicio) =====

public class InMemoryAgendaRepository : IAgendaRepository
{
    private readonly List<Agenda> _agendas = new();
    public Task<Agenda?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_agendas.FirstOrDefault(a => a.Id == id));
    public Task<Agenda?> GetByProfesionalIdAsync(Guid profesionalId, CancellationToken ct = default)
        => Task.FromResult(_agendas.FirstOrDefault(a => a.ProfesionalId == profesionalId));
    public Task<List<Agenda>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult(_agendas.ToList());
    public Task AddAsync(Agenda agenda, CancellationToken ct = default)
    { _agendas.Add(agenda); return Task.CompletedTask; }
    public Task UpdateAsync(Agenda agenda, CancellationToken ct = default)
        => Task.CompletedTask; // ya esta en memoria, las modificaciones se reflejan directamente
}

public class InMemoryCitaRepository : ICitaRepository
{
    private readonly IAgendaRepository _repo;
    public InMemoryCitaRepository(IAgendaRepository repo) { _repo = repo; }

    public async Task<Cita?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var agendas = await _repo.GetAllAsync(ct);
        return agendas.SelectMany(a => a.Citas).FirstOrDefault(c => c.Id == id);
    }
    public async Task<List<Cita>> GetByMascotaIdAsync(Guid mascotaId, CancellationToken ct = default)
    {
        var agendas = await _repo.GetAllAsync(ct);
        return agendas.SelectMany(a => a.Citas).Where(c => c.MascotaId == mascotaId).ToList();
    }
    public async Task<List<Cita>> GetByFechaAsync(DateTime fecha, CancellationToken ct = default)
    {
        var agendas = await _repo.GetAllAsync(ct);
        return agendas.SelectMany(a => a.Citas).Where(c => c.Horario.Fecha.Date == fecha.Date).ToList();
    }
}

public class InMemoryMascotaRepository : IMascotaRepository
{
    private readonly List<Mascota> _mascotas = new();
    public Task<Mascota?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_mascotas.FirstOrDefault(m => m.Id == id));
    public Task<List<Mascota>> GetByClienteIdAsync(Guid clienteId, CancellationToken ct = default)
        => Task.FromResult(_mascotas.Where(m => m.ClienteId == clienteId).ToList());
    public Task AddAsync(Mascota mascota, CancellationToken ct = default)
    { _mascotas.Add(mascota); return Task.CompletedTask; }
}

public class InMemoryFacturaRepository : IFacturaRepository
{
    private readonly List<Factura> _facturas = new();
    public Task<Factura?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_facturas.FirstOrDefault(f => f.Id == id));
    public Task<Factura?> GetByCitaIdAsync(Guid citaId, CancellationToken ct = default)
        => Task.FromResult(_facturas.FirstOrDefault(f => f.CitaId == citaId));
    public Task AddAsync(Factura factura, CancellationToken ct = default)
    { _facturas.Add(factura); return Task.CompletedTask; }
    public Task UpdateAsync(Factura factura, CancellationToken ct = default)
        => Task.CompletedTask;
}

public class InMemoryNotificacionRepository : INotificacionRepository
{
    private readonly List<Notificacion> _notificaciones = new();
    public Task<Notificacion?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_notificaciones.FirstOrDefault(n => n.Id == id));
    public Task<List<Notificacion>> GetPendientesAsync(CancellationToken ct = default)
        => Task.FromResult(_notificaciones.Where(n => n.Estado == EstadoNotificacion.Pendiente).ToList());
    public Task AddAsync(Notificacion notificacion, CancellationToken ct = default)
    { _notificaciones.Add(notificacion); return Task.CompletedTask; }
    public Task UpdateAsync(Notificacion notificacion, CancellationToken ct = default)
        => Task.CompletedTask;
}

// ===== Servicios de infraestructura para consola =====

public class ConsoleEventPublisher : IEventPublisher
{
    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken ct = default)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"    [Evento] {domainEvent.GetType().Name} - {domainEvent.OccurredOn:HH:mm:ss}");
        Console.ResetColor();
        return Task.CompletedTask;
    }
    public async Task PublishAllAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default)
    { foreach (var e in events) await PublishAsync(e, ct); }
}

public class ConsoleNotificacionService : INotificacionService
{
    public Task<bool> EnviarAsync(Notificacion notificacion, CancellationToken ct = default)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"    [EMAIL] Para: {notificacion.Destinatario.Email} | {notificacion.Asunto}: {notificacion.Mensaje}");
        Console.ResetColor();
        return Task.FromResult(true);
    }
}
