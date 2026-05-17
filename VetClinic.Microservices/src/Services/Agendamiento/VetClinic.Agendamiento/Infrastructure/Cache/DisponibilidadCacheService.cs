namespace VetClinic.Agendamiento.Infrastructure.Cache;
using Microsoft.Extensions.Caching.Memory;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.Agendamiento.Domain.ValueObjects;

/// <summary>
/// Servicio de cache para disponibilidad.
/// Evita recalcular slots en cada peticion.
/// </summary>
public class DisponibilidadCacheService
{
    private readonly IMemoryCache _cache;
    private readonly IAgendaRepository _repo;

    public DisponibilidadCacheService(IMemoryCache cache, IAgendaRepository repo)
    { _cache = cache; _repo = repo; }

    public async Task<List<HorarioDisponible>> GetDisponibilidadAsync(
        Guid profesionalId, DateTime fecha, TimeSpan duracion, CancellationToken ct = default)
    {
        var key = $"disp:{profesionalId}:{fecha:yyyyMMdd}:{duracion.TotalMinutes}";

        if (_cache.TryGetValue(key, out List<HorarioDisponible>? cached) && cached != null)
            return cached;

        var agenda = await _repo.GetByProfesionalIdAsync(profesionalId, ct);
        var slots = agenda?.ObtenerDisponibilidad(fecha, duracion) ?? new();

        _cache.Set(key, slots, TimeSpan.FromMinutes(5));
        return slots;
    }

    public void InvalidarCache(Guid profesionalId, DateTime fecha)
    {
        // Invalidar todas las duraciones posibles
        foreach (var dur in new[] { 20, 25, 30, 40, 45, 50, 60, 90 })
            _cache.Remove($"disp:{profesionalId}:{fecha:yyyyMMdd}:{dur}");
    }
}
