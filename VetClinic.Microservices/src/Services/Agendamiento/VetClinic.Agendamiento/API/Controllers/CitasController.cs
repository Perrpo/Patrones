namespace VetClinic.Agendamiento.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Agendamiento.Application.DTOs;
using VetClinic.Agendamiento.Application.UseCases;
using VetClinic.Agendamiento.Application.Validators;

/// <summary>
/// Controller DELGADO: recibe HTTP, valida entrada, despacha a Use Cases, retorna DTOs.
/// NO contiene logica de negocio ni acceso directo a repositorios.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CitasController : ControllerBase
{
    private readonly AgendarCitaUseCase _agendarCita;
    private readonly ConfirmarCitaUseCase _confirmarCita;
    private readonly CancelarCitaUseCase _cancelarCita;
    private readonly ObtenerCitaUseCase _obtenerCita;

    public CitasController(
        AgendarCitaUseCase agendarCita, ConfirmarCitaUseCase confirmarCita,
        CancelarCitaUseCase cancelarCita, ObtenerCitaUseCase obtenerCita)
    {
        _agendarCita = agendarCita;
        _confirmarCita = confirmarCita;
        _cancelarCita = cancelarCita;
        _obtenerCita = obtenerCita;
    }

    /// <summary>Crear una nueva cita.</summary>
    [HttpPost]
    public async Task<IActionResult> CrearCita([FromBody] CrearCitaRequest request, CancellationToken ct)
    {
        // Validacion de datos de entrada en el punto de recepcion (Boundary)
        var errores = CrearCitaValidator.Validar(request);
        if (errores.Count > 0)
            return BadRequest(new { errores });

        var result = await _agendarCita.EjecutarAsync(request, ct);
        return CreatedAtAction(nameof(ObtenerCita), new { id = result.Id }, result);
    }

    /// <summary>Obtener detalle de una cita por ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerCita(Guid id, CancellationToken ct)
    {
        var result = await _obtenerCita.EjecutarAsync(id, ct);
        return Ok(result);
    }

    /// <summary>Confirmar una cita pendiente.</summary>
    [HttpPut("{agendaId:guid}/citas/{citaId:guid}/confirmar")]
    public async Task<IActionResult> ConfirmarCita(Guid agendaId, Guid citaId, CancellationToken ct)
    {
        var result = await _confirmarCita.EjecutarAsync(agendaId, citaId, ct);
        return Ok(result);
    }

    /// <summary>Cancelar una cita.</summary>
    [HttpPut("{agendaId:guid}/citas/{citaId:guid}/cancelar")]
    public async Task<IActionResult> CancelarCita(Guid agendaId, Guid citaId, CancellationToken ct)
    {
        var result = await _cancelarCita.EjecutarAsync(agendaId, citaId, ct);
        return Ok(result);
    }
}
