namespace VetClinic.Personal.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using VetClinic.Personal.Domain.Interfaces;

/// <summary>
/// Expone endpoints REST para consultar el equipo de profesionales de la clínica.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProfesionalesController : ControllerBase
{
    private readonly IProfesionalRepository _profesionalRepo;

    public ProfesionalesController(IProfesionalRepository profesionalRepo)
    {
        _profesionalRepo = profesionalRepo;
    }

    /// <summary>Obtiene todos los profesionales activos.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var profesionales = await _profesionalRepo.GetAllAsync(ct);
        var result = profesionales.Select(p => new
        {
            p.Id,
            p.Nombre,
            p.Rol,
            Especialidad = p.Especialidad.Nombre
        });
        return Ok(result);
    }

    /// <summary>Obtiene un profesional por su ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var profesional = await _profesionalRepo.GetByIdAsync(id, ct);
        if (profesional == null) return NotFound(new { mensaje = "Profesional no encontrado." });
        return Ok(new
        {
            profesional.Id,
            profesional.Nombre,
            profesional.Rol,
            Especialidad = profesional.Especialidad.Nombre
        });
    }
}
