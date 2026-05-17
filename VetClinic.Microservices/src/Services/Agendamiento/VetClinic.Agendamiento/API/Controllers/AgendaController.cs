namespace VetClinic.Agendamiento.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Agendamiento.Application.UseCases;

/// <summary>Controller delgado para consultar agendas y disponibilidad.</summary>
[ApiController]
[Route("api/[controller]")]
public class AgendaController : ControllerBase
{
    private readonly ConsultarAgendaUseCase _consultarAgenda;
    private readonly ConsultarDisponibilidadUseCase _consultarDisponibilidad;

    public AgendaController(ConsultarAgendaUseCase consultarAgenda, ConsultarDisponibilidadUseCase consultarDisponibilidad)
    { _consultarAgenda = consultarAgenda; _consultarDisponibilidad = consultarDisponibilidad; }

    [HttpGet("{profesionalId:guid}")]
    public async Task<IActionResult> ConsultarAgenda(Guid profesionalId, CancellationToken ct)
    {
        var result = await _consultarAgenda.EjecutarAsync(profesionalId, ct);
        return Ok(result);
    }

    [HttpGet("{profesionalId:guid}/disponibilidad")]
    public async Task<IActionResult> ConsultarDisponibilidad(Guid profesionalId, [FromQuery] DateTime fecha, [FromQuery] int duracionMinutos = 30, CancellationToken ct = default)
    {
        var result = await _consultarDisponibilidad.EjecutarAsync(profesionalId, fecha, duracionMinutos, ct);
        return Ok(result);
    }
}
