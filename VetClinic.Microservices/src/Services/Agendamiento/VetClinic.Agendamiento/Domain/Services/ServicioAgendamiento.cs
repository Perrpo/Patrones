namespace VetClinic.Agendamiento.Domain.Services;

using VetClinic.Agendamiento.Domain.Aggregates;
using VetClinic.Agendamiento.Domain.Entities;
using VetClinic.Agendamiento.Domain.ValueObjects;
using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Servicio de Dominio: encapsula lógica de negocio que involucra múltiples agregados.
/// Valida disponibilidad cruzando Agenda del profesional + citas de la mascota.
/// No depende de infraestructura (no usa repos, BD, etc.).
/// </summary>
public class ServicioAgendamiento
{
    /// <summary>
    /// Valida que se pueda agendar una cita verificando:
    /// 1) No hay conflicto en la agenda del profesional
    /// 2) La mascota no tiene otra cita en el mismo horario
    /// </summary>
    public void ValidarDisponibilidad(
        Agenda agenda,
        Guid mascotaId,
        HorarioDisponible horario,
        IReadOnlyList<Cita> citasMascota)
    {
        // Validar conflicto en la agenda del profesional
        if (agenda.HayConflicto(horario.Fecha, horario.HoraInicio, horario.Duracion))
            throw new DomainException("El profesional ya tiene una cita en ese horario.");

        // Validar conflicto con citas de la mascota
        var conflictoMascota = citasMascota.Any(c =>
            c.EstaActiva() &&
            c.Horario.Fecha.Date == horario.Fecha.Date &&
            !(horario.HoraFin <= c.Horario.HoraInicio || horario.HoraInicio >= c.Horario.HoraFin));

        if (conflictoMascota)
            throw new DomainException("La mascota ya tiene una cita en ese horario.");
    }
}
