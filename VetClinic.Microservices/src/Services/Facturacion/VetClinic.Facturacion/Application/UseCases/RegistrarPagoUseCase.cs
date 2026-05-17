namespace VetClinic.Facturacion.Application.UseCases;
using VetClinic.Facturacion.Application.DTOs;
using VetClinic.Facturacion.Domain.Interfaces;
using VetClinic.BuildingBlocks.Application;
using VetClinic.BuildingBlocks.Domain;

public class RegistrarPagoUseCase
{
    private readonly IFacturaRepository _facturaRepo;
    private readonly IEventPublisher _eventPublisher;
    public RegistrarPagoUseCase(IFacturaRepository repo, IEventPublisher pub)
    { _facturaRepo = repo; _eventPublisher = pub; }

    public async Task<PagoDto> EjecutarAsync(Guid facturaId, RegistrarPagoRequest request, CancellationToken ct)
    {
        var factura = await _facturaRepo.GetByIdAsync(facturaId, ct)
            ?? throw new DomainException("Factura no encontrada.");
        var pago = factura.RegistrarPago(request.Monto, request.MetodoPago);
        await _facturaRepo.UpdateAsync(factura, ct);
        await _eventPublisher.PublishAllAsync(factura.DomainEvents, ct);
        factura.ClearDomainEvents();
        return new PagoDto(pago.Id, pago.Monto.Cantidad, pago.Metodo.Tipo, pago.Estado.ToString());
    }
}
