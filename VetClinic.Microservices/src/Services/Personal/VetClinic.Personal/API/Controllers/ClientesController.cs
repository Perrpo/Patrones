namespace VetClinic.Personal.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using VetClinic.Personal.Application.UseCases;

/// <summary>
/// Controller DELGADO: recibe HTTP, valida entrada, despacha a Use Cases, retorna DTOs.
/// NO contiene lógica de negocio ni acceso directo a repositorios.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ObtenerClientesUseCase _obtenerClientes;
    private readonly ObtenerClientePorIdUseCase _obtenerClientePorId;

    public ClientesController(
        ObtenerClientesUseCase obtenerClientes,
        ObtenerClientePorIdUseCase obtenerClientePorId)
    {
        _obtenerClientes = obtenerClientes;
        _obtenerClientePorId = obtenerClientePorId;
    }

    /// <summary>Obtiene todos los clientes registrados. Retorna DTOs, nunca entidades de dominio.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _obtenerClientes.EjecutarAsync(ct);
        return Ok(result);
    }

    /// <summary>Obtiene un cliente por su ID. Retorna 404 si no existe.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _obtenerClientePorId.EjecutarAsync(id, ct);
        return Ok(result);
    }
}
