namespace VetClinic.Personal.Domain.ValueObjects;
using VetClinic.BuildingBlocks.Domain;
/// <summary>Value Object inmutable para email con validacion.</summary>
public class Email : ValueObject
{
    public string Direccion { get; }
    public Email(string direccion)
    {
        if (string.IsNullOrWhiteSpace(direccion)) throw new DomainException("El email es obligatorio.");
        if (!direccion.Contains('@')) throw new DomainException("El email debe contener @.");
        Direccion = direccion;
    }
    protected override IEnumerable<object?> GetEqualityComponents() { yield return Direccion; }
}
