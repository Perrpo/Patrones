namespace VetClinic.Notificaciones.Application.Consumers;
using VetClinic.Notificaciones.Domain.Entities;
using VetClinic.Notificaciones.Domain.Enums;
using VetClinic.Notificaciones.Domain.Interfaces;
using VetClinic.Notificaciones.Domain.ValueObjects;

/// <summary>
/// Consumer: escucha eventos CitaConfirmada y crea notificacion.
/// No contiene logica de negocio del dominio.
/// </summary>
public class CitaConfirmadaConsumer
{
    private readonly INotificacionRepository _repo;

    public CitaConfirmadaConsumer(INotificacionRepository repo) { _repo = repo; }

    public async Task HandleAsync(Guid citaId, string nombreCliente, string emailCliente, DateTime fechaCita, CancellationToken ct = default)
    {
        var destinatario = new Destinatario(nombreCliente, emailCliente);
        var notificacion = new Notificacion(
            Guid.NewGuid(),
            destinatario,
            "Cita Confirmada",
            $"Su cita del {fechaCita:dd/MM/yyyy HH:mm} ha sido confirmada.",
            TipoNotificacion.Email
        );

        await _repo.AddAsync(notificacion, ct);
    }
}
