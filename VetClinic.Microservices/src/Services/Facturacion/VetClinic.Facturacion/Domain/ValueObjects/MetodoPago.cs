namespace VetClinic.Facturacion.Domain.ValueObjects;
using VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Value Object inmutable para el metodo de pago.
/// Tiene Tipo y Descripcion como indica el documento de diseno.
/// </summary>
public class MetodoPago : ValueObject
{
    private static readonly Dictionary<string, string> MetodosValidos = new()
    {
        { "Efectivo", "Pago en efectivo en caja" },
        { "Tarjeta", "Pago con tarjeta debito/credito" },
        { "Transferencia", "Transferencia bancaria electronica" }
    };

    public string Tipo { get; }
    public string Descripcion { get; }

    public MetodoPago(string tipo)
    {
        if (!MetodosValidos.ContainsKey(tipo))
            throw new DomainException($"Metodo de pago invalido: {tipo}. Validos: {string.Join(", ", MetodosValidos.Keys)}");
        Tipo = tipo;
        Descripcion = MetodosValidos[tipo];
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Tipo;
        yield return Descripcion;
    }
}
