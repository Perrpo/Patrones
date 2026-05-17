namespace VetClinic.Agendamiento.Infrastructure.Services;
using VetClinic.BuildingBlocks.Application;
using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Implementacion en memoria del publicador de eventos.
/// En produccion se reemplazaria por RabbitMQ, Kafka, etc.
/// </summary>
public class InMemoryEventPublisher : IEventPublisher
{
    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken ct = default)
    {
        Console.WriteLine($"[Evento] {domainEvent.GetType().Name} - {domainEvent.OccurredOn:HH:mm:ss}");
        return Task.CompletedTask;
    }

    public async Task PublishAllAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default)
    {
        foreach (var e in events) await PublishAsync(e, ct);
    }
}
