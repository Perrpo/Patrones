namespace VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Contrato para eventos de dominio.
/// Representan algo que YA sucedió en el dominio. Se nombran en pasado.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
