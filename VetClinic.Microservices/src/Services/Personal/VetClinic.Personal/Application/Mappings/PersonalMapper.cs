namespace VetClinic.Personal.Application.Mappings;

using VetClinic.Personal.Application.DTOs;
using VetClinic.Personal.Domain.Aggregates;
using VetClinic.Personal.Domain.Entities;

/// <summary>
/// Mapeo explícito entre entidades de dominio y DTOs.
/// No contiene lógica de negocio.
/// </summary>
public static class PersonalMapper
{
    public static ClienteDto ToDto(Cliente cliente) => new(
        cliente.Id,
        cliente.Nombre,
        cliente.Email.Direccion,
        cliente.Mascotas.Count,
        cliente.Mascotas.Select(m => new MascotaDto(m.Id, m.Nombre, m.Especie, m.Raza, m.Edad)).ToList()
    );

    public static ProfesionalDto ToDto(Profesional profesional) => new(
        profesional.Id,
        profesional.Nombre,
        profesional.Email.Direccion,
        profesional.Rol,
        profesional.Especialidad?.Nombre
    );
}
