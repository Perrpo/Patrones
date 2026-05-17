namespace VetClinic.ApiGateway.Controllers;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller del Gateway que enruta peticiones a los microservicios correspondientes.
/// Actua como punto de entrada unico para todos los clientes.
/// </summary>
[ApiController]
[Route("api/gateway")]
public class GatewayController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GatewayController> _logger;

    public GatewayController(IHttpClientFactory httpClientFactory, ILogger<GatewayController> logger)
    { _httpClientFactory = httpClientFactory; _logger = logger; }

    /// <summary>Enruta al microservicio de Agendamiento.</summary>
    [HttpGet("agendamiento/{**path}")]
    public async Task<IActionResult> Agendamiento(string path)
    {
        var client = _httpClientFactory.CreateClient("Agendamiento");
        var response = await client.GetAsync($"/api/{path}");
        var content = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, content);
    }

    /// <summary>Enruta al microservicio de Facturacion.</summary>
    [HttpGet("facturacion/{**path}")]
    public async Task<IActionResult> Facturacion(string path)
    {
        var client = _httpClientFactory.CreateClient("Facturacion");
        var response = await client.GetAsync($"/api/{path}");
        var content = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, content);
    }

    /// <summary>Enruta al microservicio de Personal.</summary>
    [HttpGet("personal/{**path}")]
    public async Task<IActionResult> Personal(string path)
    {
        var client = _httpClientFactory.CreateClient("Personal");
        var response = await client.GetAsync($"/api/{path}");
        var content = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, content);
    }

    /// <summary>Health check del gateway.</summary>
    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
}
