namespace VetClinic.Agendamiento.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using VetClinic.Agendamiento.Domain.Entities;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.Agendamiento.Infrastructure.Persistence;

/// <summary>Implementacion del repositorio de Mascotas.</summary>
public class MascotaRepository : IMascotaRepository
{
    private readonly AgendamientoDbContext _context;
    public MascotaRepository(AgendamientoDbContext context) => _context = context;

    public async Task<Mascota?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Mascotas.FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task<List<Mascota>> GetByClienteIdAsync(Guid clienteId, CancellationToken ct = default)
        => await _context.Mascotas.Where(m => m.ClienteId == clienteId).ToListAsync(ct);

    public async Task AddAsync(Mascota mascota, CancellationToken ct = default)
    {
        await _context.Mascotas.AddAsync(mascota, ct);
        await _context.SaveChangesAsync(ct);
    }
}
