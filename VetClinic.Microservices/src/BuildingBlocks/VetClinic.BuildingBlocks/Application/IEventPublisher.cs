namespace VetClinic.BuildingBlocks.Application;

using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Interfaz para publicar eventos de dominio.
/// Definida en BuildingBlocks, implementada en Infrastructure.
/// </summary>
public interface IEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken ct = default);
    Task PublishAllAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default);
}
