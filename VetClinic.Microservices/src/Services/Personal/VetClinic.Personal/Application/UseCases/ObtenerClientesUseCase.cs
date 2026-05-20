namespace VetClinic.Personal.Application.UseCases;

using VetClinic.Personal.Application.DTOs;
using VetClinic.Personal.Application.Mappings;
using VetClinic.Personal.Domain.Interfaces;
using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Caso de uso: Obtener todos los clientes registrados.
/// Orquesta: obtiene datos del repositorio, mapea a DTOs, retorna al controller.
/// NO contiene lógica de negocio (esa está en los agregados).
/// </summary>
public class ObtenerClientesUseCase
{
    private readonly IClienteRepository _repo;

    public ObtenerClientesUseCase(IClienteRepository repo) => _repo = repo;

    public async Task<List<ClienteDto>> EjecutarAsync(CancellationToken ct = default)
    {
        var clientes = await _repo.GetAllAsync(ct);
        return clientes.Select(PersonalMapper.ToDto).ToList();
    }
}

/// <summary>
/// Caso de uso: Obtener un cliente por su ID.
/// Lanza DomainException si no se encuentra.
/// </summary>
public class ObtenerClientePorIdUseCase
{
    private readonly IClienteRepository _repo;

    public ObtenerClientePorIdUseCase(IClienteRepository repo) => _repo = repo;

    public async Task<ClienteDto> EjecutarAsync(Guid id, CancellationToken ct = default)
    {
        var cliente = await _repo.GetByIdAsync(id, ct)
            ?? throw new DomainException($"Cliente con ID '{id}' no encontrado.");
        return PersonalMapper.ToDto(cliente);
    }
}
