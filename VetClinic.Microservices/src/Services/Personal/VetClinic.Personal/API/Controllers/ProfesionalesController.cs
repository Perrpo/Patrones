namespace VetClinic.Personal.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using VetClinic.Personal.Application.UseCases;

/// <summary>
/// Controller DELGADO: recibe HTTP, delega a Use Cases, retorna DTOs.
/// NO accede directamente a repositorios ni contiene lógica de negocio.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProfesionalesController : ControllerBase
{
    private readonly ObtenerProfesionalesUseCase _obtenerProfesionales;
    private readonly ObtenerProfesionalPorIdUseCase _obtenerProfesionalPorId;

    public ProfesionalesController(
        ObtenerProfesionalesUseCase obtenerProfesionales,
        ObtenerProfesionalPorIdUseCase obtenerProfesionalPorId)
    {
        _obtenerProfesionales = obtenerProfesionales;
        _obtenerProfesionalPorId = obtenerProfesionalPorId;
    }

    /// <summary>Obtiene todos los profesionales activos. Retorna DTOs, nunca entidades.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _obtenerProfesionales.EjecutarAsync(ct);
        return Ok(result);
    }

    /// <summary>Obtiene un profesional por su ID. Retorna 404 si no existe.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _obtenerProfesionalPorId.EjecutarAsync(id, ct);
        return Ok(result);
    }
}
