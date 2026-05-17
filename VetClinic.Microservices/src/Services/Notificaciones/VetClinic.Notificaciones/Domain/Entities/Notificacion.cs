namespace VetClinic.Notificaciones.Domain.Entities;
using VetClinic.BuildingBlocks.Domain;
using VetClinic.Notificaciones.Domain.Enums;
using VetClinic.Notificaciones.Domain.ValueObjects;

/// <summary>
/// Entidad enriquecida: Notificacion con comportamiento de negocio.
/// Controla su ciclo de vida (Pendiente -> Enviada / Fallida).
/// </summary>
public class Notificacion : Entity<Guid>
{
    public Destinatario Destinatario { get; private set; }
    public string Asunto { get; private set; }
    public string Mensaje { get; private set; }
    public TipoNotificacion Tipo { get; private set; }
    public EstadoNotificacion Estado { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime? FechaEnvio { get; private set; }

    public Notificacion(Guid id, Destinatario destinatario, string asunto, string mensaje, TipoNotificacion tipo)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(asunto))
            throw new DomainException("El asunto es obligatorio.");
        if (string.IsNullOrWhiteSpace(mensaje))
            throw new DomainException("El mensaje es obligatorio.");

        Destinatario = destinatario;
        Asunto = asunto;
        Mensaje = mensaje;
        Tipo = tipo;
        Estado = EstadoNotificacion.Pendiente;
        FechaCreacion = DateTime.UtcNow;
    }

    /// <summary>Marca la notificacion como enviada exitosamente.</summary>
    public void MarcarComoEnviada()
    {
        if (Estado != EstadoNotificacion.Pendiente)
            throw new DomainException("Solo se pueden enviar notificaciones pendientes.");
        Estado = EstadoNotificacion.Enviada;
        FechaEnvio = DateTime.UtcNow;
    }

    /// <summary>Marca la notificacion como fallida.</summary>
    public void MarcarComoFallida()
    {
        if (Estado != EstadoNotificacion.Pendiente)
            throw new DomainException("Solo se pueden marcar como fallidas notificaciones pendientes.");
        Estado = EstadoNotificacion.Fallida;
    }

    public bool EstaPendiente() => Estado == EstadoNotificacion.Pendiente;

    private Notificacion() : base(Guid.Empty) { Destinatario = null!; Asunto = ""; Mensaje = ""; }
}
