namespace VetClinic.Facturacion.Application.DTOs;
public record FacturaDto(Guid Id, Guid CitaId, decimal Total, bool EstaPagada, List<PagoDto> Pagos);
public record PagoDto(Guid Id, decimal Monto, string Metodo, string Estado);
public record RegistrarPagoRequest(decimal Monto, string MetodoPago);
