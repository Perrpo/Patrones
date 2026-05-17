namespace VetClinic.Notificaciones.Application.Consumers;
using VetClinic.Notificaciones.Domain.Entities;
using VetClinic.Notificaciones.Domain.Enums;
using VetClinic.Notificaciones.Domain.Interfaces;
using VetClinic.Notificaciones.Domain.ValueObjects;

/// <summary>Consumer: escucha eventos PagoRegistrado y crea notificacion.</summary>
public class PagoRegistradoConsumer
{
    private readonly INotificacionRepository _repo;

    public PagoRegistradoConsumer(INotificacionRepository repo) { _repo = repo; }

    public async Task HandleAsync(Guid pagoId, string nombreCliente, string emailCliente, decimal monto, CancellationToken ct = default)
    {
        var destinatario = new Destinatario(nombreCliente, emailCliente);
        var notificacion = new Notificacion(
            Guid.NewGuid(),
            destinatario,
            "Pago Registrado",
            $"Su pago por ${monto:N0} ha sido registrado exitosamente.",
            TipoNotificacion.Email
        );

        await _repo.AddAsync(notificacion, ct);
    }
}
