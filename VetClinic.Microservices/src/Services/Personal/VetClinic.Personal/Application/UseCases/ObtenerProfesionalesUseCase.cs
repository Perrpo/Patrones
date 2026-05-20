namespace VetClinic.Personal.Application.UseCases;

using VetClinic.Personal.Application.DTOs;
using VetClinic.Personal.Application.Mappings;
using VetClinic.Personal.Domain.Interfaces;
using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Caso de uso: Obtener todos los profesionales de la clínica.
/// Orquesta: repositorio → mapeo → DTO. Sin lógica de negocio propia.
/// </summary>
public class ObtenerProfesionalesUseCase
{
    private readonly IProfesionalRepository _repo;

    public ObtenerProfesionalesUseCase(IProfesionalRepository repo) => _repo = repo;

    public async Task<List<ProfesionalDto>> EjecutarAsync(CancellationToken ct = default)
    {
        var profesionales = await _repo.GetAllAsync(ct);
        return profesionales.Select(PersonalMapper.ToDto).ToList();
    }
}

/// <summary>
/// Caso de uso: Obtener un profesional por su ID.
/// Lanza DomainException si no se encuentra.
/// </summary>
public class ObtenerProfesionalPorIdUseCase
{
    private readonly IProfesionalRepository _repo;

    public ObtenerProfesionalPorIdUseCase(IProfesionalRepository repo) => _repo = repo;

    public async Task<ProfesionalDto> EjecutarAsync(Guid id, CancellationToken ct = default)
    {
        var profesional = await _repo.GetByIdAsync(id, ct)
            ?? throw new DomainException($"Profesional con ID '{id}' no encontrado.");
        return PersonalMapper.ToDto(profesional);
    }
}
