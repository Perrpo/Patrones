namespace VetClinic.Personal.Application.DTOs;

/// <summary>
/// DTO de salida: datos de un profesional de la clínica.
/// Transporta datos entre capas sin exponer la entidad de dominio.
/// </summary>
public record ProfesionalDto(Guid Id, string Nombre, string Email, string Rol, string? Especialidad);

/// <summary>
/// DTO de entrada: datos para crear/actualizar un profesional.
/// Validación básica en el punto de entrada (Boundary).
/// </summary>
public record CrearProfesionalRequest(string Nombre, string Email, string Rol, string? Especialidad);
