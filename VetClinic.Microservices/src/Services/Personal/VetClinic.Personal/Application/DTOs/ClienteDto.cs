namespace VetClinic.Personal.Application.DTOs;

/// <summary>
/// DTO de salida: resumen de mascota del cliente.
/// Transporta datos entre la capa de Application y la capa API. Sin comportamiento.
/// </summary>
public record MascotaDto(Guid Id, string Nombre, string Especie, string Raza, int Edad);

/// <summary>
/// DTO de salida: datos de un cliente con sus mascotas.
/// Nunca expone la entidad de dominio directamente.
/// </summary>
public record ClienteDto(Guid Id, string Nombre, string Email, int TotalMascotas, List<MascotaDto> Mascotas);

/// <summary>
/// DTO de entrada: datos para crear/actualizar un cliente.
/// Validación de datos de entrada en el punto de recepción (Boundary).
/// </summary>
public record CrearClienteRequest(string Nombre, string Email);
