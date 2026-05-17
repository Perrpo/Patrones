namespace VetClinic.Agendamiento.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using VetClinic.Agendamiento.Domain.Aggregates;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.Agendamiento.Infrastructure.Persistence;

/// <summary>
/// Implementacion concreta del repositorio de Agenda.
/// Implementa la interfaz definida en Domain.
/// Usa el DbContext para operaciones CRUD.
/// Retorna entidades y agregados del dominio, no DTOs.
/// </summary>
public class AgendaRepository : IAgendaRepository
{
    private readonly AgendamientoDbContext _context;
    public AgendaRepository(AgendamientoDbContext context) => _context = context;

    public async Task<Agenda?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Agendas
            .Include(a => a.Citas).Include(a => a.HorariosLaborales)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<Agenda?> GetByProfesionalIdAsync(Guid profesionalId, CancellationToken ct = default)
        => await _context.Agendas
            .Include(a => a.Citas).Include(a => a.HorariosLaborales)
            .FirstOrDefaultAsync(a => a.ProfesionalId == profesionalId, ct);

    public async Task<List<Agenda>> GetAllAsync(CancellationToken ct = default)
        => await _context.Agendas.Include(a => a.Citas).ToListAsync(ct);

    public async Task AddAsync(Agenda agenda, CancellationToken ct = default)
    {
        await _context.Agendas.AddAsync(agenda, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Agenda agenda, CancellationToken ct = default)
    {
        _context.Agendas.Update(agenda);
        await _context.SaveChangesAsync(ct);
    }
}
