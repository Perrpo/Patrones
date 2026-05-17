namespace VetClinic.Notificaciones.Worker;
using VetClinic.Notificaciones.Application.UseCases;

/// <summary>
/// BackgroundService que se ejecuta en segundo plano.
/// Procesa notificaciones pendientes cada 10 segundos.
/// Descarga la responsabilidad de envios sincronos desde la API.
/// </summary>
public class NotificacionWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificacionWorker> _logger;

    public NotificacionWorker(IServiceProvider serviceProvider, ILogger<NotificacionWorker> logger)
    { _serviceProvider = serviceProvider; _logger = logger; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NotificacionWorker iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var useCase = scope.ServiceProvider.GetRequiredService<ProcesarPendientesUseCase>();
                var enviadas = await useCase.EjecutarAsync(stoppingToken);

                if (enviadas > 0)
                    _logger.LogInformation("{Count} notificaciones enviadas.", enviadas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando notificaciones.");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
