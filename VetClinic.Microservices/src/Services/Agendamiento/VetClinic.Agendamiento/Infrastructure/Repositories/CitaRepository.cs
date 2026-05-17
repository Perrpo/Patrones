namespace VetClinic.Agendamiento.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using VetClinic.Agendamiento.Domain.Entities;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.Agendamiento.Infrastructure.Persistence;

/// <summary>Implementacion del repositorio de Citas.</summary>
public class CitaRepository : ICitaRepository
{
    private readonly AgendamientoDbContext _context;
    public CitaRepository(AgendamientoDbContext context) => _context = context;

    public async Task<Cita?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Citas.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<List<Cita>> GetByMascotaIdAsync(Guid mascotaId, CancellationToken ct = default)
        => await _context.Citas.Where(c => c.MascotaId == mascotaId).ToListAsync(ct);

    public async Task<List<Cita>> GetByFechaAsync(DateTime fecha, CancellationToken ct = default)
        => await _context.Citas.Where(c => c.Horario.Fecha == fecha.Date).ToListAsync(ct);
}
