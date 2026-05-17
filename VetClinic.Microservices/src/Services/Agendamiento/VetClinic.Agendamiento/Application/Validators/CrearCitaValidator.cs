namespace VetClinic.Agendamiento.Application.Validators;
using VetClinic.Agendamiento.Application.DTOs;

/// <summary>
/// Validacion basica de entrada (boundaries). NO reemplaza las reglas de negocio del dominio.
/// Valida forma e integridad de datos antes de llegar al Handler.
/// </summary>
public static class CrearCitaValidator
{
    public static List<string> Validar(CrearCitaRequest request)
    {
        var errores = new List<string>();
        if (request.ProfesionalId == Guid.Empty)
            errores.Add("El profesional es obligatorio.");
        if (request.MascotaId == Guid.Empty)
            errores.Add("La mascota es obligatoria.");
        if (request.Fecha.Date < DateTime.Today)
            errores.Add("La fecha no puede ser anterior a hoy.");
        if (request.HoraFin <= request.HoraInicio)
            errores.Add("La hora fin debe ser mayor a la hora inicio.");
        return errores;
    }
}
