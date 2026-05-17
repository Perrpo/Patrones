namespace VetClinic.Facturacion.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Facturacion.Application.DTOs;
using VetClinic.Facturacion.Application.UseCases;
[ApiController][Route("api/[controller]")]
public class PagosController : ControllerBase
{
    private readonly RegistrarPagoUseCase _registrarPago;
    public PagosController(RegistrarPagoUseCase registrarPago) { _registrarPago = registrarPago; }

    [HttpPost("{facturaId:guid}")]
    public async Task<IActionResult> RegistrarPago(Guid facturaId, [FromBody] RegistrarPagoRequest request, CancellationToken ct)
    {
        var result = await _registrarPago.EjecutarAsync(facturaId, request, ct);
        return Ok(result);
    }
}
