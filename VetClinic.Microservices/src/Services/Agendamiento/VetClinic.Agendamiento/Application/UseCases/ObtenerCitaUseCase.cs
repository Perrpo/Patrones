namespace VetClinic.Agendamiento.Application.UseCases;
using VetClinic.Agendamiento.Application.DTOs;
using VetClinic.Agendamiento.Application.Mappings;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.BuildingBlocks.Domain;

/// <summary>Caso de uso (lectura): Obtener detalle de una cita por ID.</summary>
public class ObtenerCitaUseCase
{
    private readonly ICitaRepository _citaRepo;
    public ObtenerCitaUseCase(ICitaRepository citaRepo) { _citaRepo = citaRepo; }

    public async Task<CitaDto> EjecutarAsync(Guid citaId, CancellationToken ct = default)
    {
        var cita = await _citaRepo.GetByIdAsync(citaId, ct)
            ?? throw new DomainException("Cita no encontrada.");
        return AgendamientoMapper.ToDto(cita);
    }
}
