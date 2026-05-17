namespace VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Excepción base del dominio para violaciones de reglas de negocio.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}
