namespace VetClinic.Personal.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using VetClinic.Personal.Domain.Interfaces;

/// <summary>
/// Expone endpoints REST para consultar clientes y sus mascotas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteRepository _clienteRepo;

    public ClientesController(IClienteRepository clienteRepo)
    {
        _clienteRepo = clienteRepo;
    }

    /// <summary>Obtiene todos los clientes registrados.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var clientes = await _clienteRepo.GetAllAsync(ct);
        var result = clientes.Select(c => new
        {
            c.Id,
            c.Nombre,
            Email = c.Email.Value,
            TotalMascotas = c.Mascotas.Count,
            Mascotas = c.Mascotas.Select(m => new { m.Id, m.Nombre, m.Especie, m.Raza, m.Edad })
        });
        return Ok(result);
    }

    /// <summary>Obtiene un cliente por su ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var cliente = await _clienteRepo.GetByIdAsync(id, ct);
        if (cliente == null) return NotFound(new { mensaje = "Cliente no encontrado." });
        return Ok(new
        {
            cliente.Id,
            cliente.Nombre,
            Email = cliente.Email.Value,
            Mascotas = cliente.Mascotas.Select(m => new { m.Id, m.Nombre, m.Especie, m.Raza, m.Edad })
        });
    }
}
