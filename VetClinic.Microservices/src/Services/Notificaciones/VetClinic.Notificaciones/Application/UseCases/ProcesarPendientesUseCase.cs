namespace VetClinic.Notificaciones.Application.UseCases;
using VetClinic.Notificaciones.Domain.Interfaces;

/// <summary>Caso de uso: Procesar todas las notificaciones pendientes.</summary>
public class ProcesarPendientesUseCase
{
    private readonly INotificacionRepository _repo;
    private readonly INotificacionService _servicio;

    public ProcesarPendientesUseCase(INotificacionRepository repo, INotificacionService servicio)
    { _repo = repo; _servicio = servicio; }

    public async Task<int> EjecutarAsync(CancellationToken ct = default)
    {
        var pendientes = await _repo.GetPendientesAsync(ct);
        var enviadas = 0;

        foreach (var notificacion in pendientes)
        {
            var enviado = await _servicio.EnviarAsync(notificacion, ct);
            if (enviado) { notificacion.MarcarComoEnviada(); enviadas++; }
            else { notificacion.MarcarComoFallida(); }
            await _repo.UpdateAsync(notificacion, ct);
        }

        return enviadas;
    }
}
