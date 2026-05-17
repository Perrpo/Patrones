namespace VetClinic.Agendamiento.Domain.Enums;

/// <summary>
/// Estados del ciclo de vida de una cita.
/// Pendiente → Confirmada → Finalizada
/// Pendiente → Cancelada
/// Confirmada → Cancelada
/// </summary>
public enum EstadoCita
{
    Pendiente,
    Confirmada,
    Finalizada,
    Cancelada
}
