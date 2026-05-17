namespace VetClinic.Facturacion.Infrastructure.Services;
using VetClinic.BuildingBlocks.Application;
using VetClinic.BuildingBlocks.Domain;

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
