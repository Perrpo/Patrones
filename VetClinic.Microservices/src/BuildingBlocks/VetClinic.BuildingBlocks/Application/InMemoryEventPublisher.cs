using VetClinic.BuildingBlocks.Domain;

namespace VetClinic.BuildingBlocks.Application;

/// <summary>
/// Implementación en memoria del publicador de eventos de dominio.
/// Centralizada en BuildingBlocks para evitar duplicación entre microservicios.
/// Los microservicios pueden usarla directamente o reemplazarla con un bus real (RabbitMQ, etc.).
/// </summary>
public class InMemoryEventPublisher : IEventPublisher
{
    private readonly List<IDomainEvent> _published = new();

    public IReadOnlyList<IDomainEvent> PublishedEvents => _published.AsReadOnly();

    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken ct = default)
    {
        _published.Add(domainEvent);
        return Task.CompletedTask;
    }

    public async Task PublishAllAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default)
    {
        foreach (var e in events)
            await PublishAsync(e, ct);
    }
}
