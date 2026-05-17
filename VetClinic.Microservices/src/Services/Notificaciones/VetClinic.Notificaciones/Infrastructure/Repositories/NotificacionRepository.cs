namespace VetClinic.Notificaciones.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using VetClinic.Notificaciones.Domain.Entities;
using VetClinic.Notificaciones.Domain.Enums;
using VetClinic.Notificaciones.Domain.Interfaces;
using VetClinic.Notificaciones.Infrastructure.Persistence;

public class NotificacionRepository : INotificacionRepository
{
    private readonly NotificacionesDbContext _ctx;
    public NotificacionRepository(NotificacionesDbContext ctx) => _ctx = ctx;

    public async Task<Notificacion?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _ctx.Notificaciones.FirstOrDefaultAsync(n => n.Id == id, ct);

    public async Task<List<Notificacion>> GetPendientesAsync(CancellationToken ct)
        => await _ctx.Notificaciones.Where(n => n.Estado == EstadoNotificacion.Pendiente).ToListAsync(ct);

    public async Task AddAsync(Notificacion n, CancellationToken ct)
    { await _ctx.Notificaciones.AddAsync(n, ct); await _ctx.SaveChangesAsync(ct); }

    public async Task UpdateAsync(Notificacion n, CancellationToken ct)
    { _ctx.Notificaciones.Update(n); await _ctx.SaveChangesAsync(ct); }
}
