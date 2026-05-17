namespace VetClinic.Notificaciones.Application.UseCases;
using VetClinic.Notificaciones.Domain.Interfaces;
using VetClinic.BuildingBlocks.Domain;

/// <summary>Caso de uso: Enviar una notificacion pendiente.</summary>
public class EnviarNotificacionUseCase
{
    private readonly INotificacionRepository _repo;
    private readonly INotificacionService _servicio;

    public EnviarNotificacionUseCase(INotificacionRepository repo, INotificacionService servicio)
    { _repo = repo; _servicio = servicio; }

    public async Task EjecutarAsync(Guid notificacionId, CancellationToken ct = default)
    {
        var notificacion = await _repo.GetByIdAsync(notificacionId, ct)
            ?? throw new DomainException("Notificacion no encontrada.");

        var enviado = await _servicio.EnviarAsync(notificacion, ct);

        if (enviado)
            notificacion.MarcarComoEnviada();
        else
            notificacion.MarcarComoFallida();

        await _repo.UpdateAsync(notificacion, ct);
    }
}
