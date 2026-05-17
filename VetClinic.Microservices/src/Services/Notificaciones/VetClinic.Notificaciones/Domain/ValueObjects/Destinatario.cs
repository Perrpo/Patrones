namespace VetClinic.Notificaciones.Domain.ValueObjects;
using VetClinic.BuildingBlocks.Domain;
/// <summary>Value Object inmutable: datos del destinatario de la notificacion.</summary>
public class Destinatario : ValueObject
{
    public string Nombre { get; }
    public string Email { get; }

    public Destinatario(string nombre, string email)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre del destinatario es obligatorio.");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new DomainException("El email del destinatario es invalido.");
        Nombre = nombre;
        Email = email;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Nombre;
        yield return Email;
    }

    // Constructor privado para EF Core
    private Destinatario() { Nombre = ""; Email = ""; }
}
