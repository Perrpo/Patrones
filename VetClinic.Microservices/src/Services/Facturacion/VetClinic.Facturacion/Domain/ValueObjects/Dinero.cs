namespace VetClinic.Facturacion.Domain.ValueObjects;
using VetClinic.BuildingBlocks.Domain;
/// <summary>Value Object inmutable para cantidades monetarias.</summary>
public class Dinero : ValueObject
{
    public decimal Cantidad { get; }
    public string Moneda { get; }
    public Dinero(decimal cantidad, string moneda = "COP")
    {
        if (cantidad < 0) throw new DomainException("El monto no puede ser negativo.");
        if (string.IsNullOrWhiteSpace(moneda)) throw new DomainException("La moneda es obligatoria.");
        Cantidad = cantidad; Moneda = moneda;
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    { yield return Cantidad; yield return Moneda; }
}
