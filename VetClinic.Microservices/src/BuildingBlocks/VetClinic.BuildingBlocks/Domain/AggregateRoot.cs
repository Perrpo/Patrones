namespace VetClinic.BuildingBlocks.Domain;

/// <summary>
/// Clase base para las raíces de agregado.
/// El agregado es el límite de consistencia transaccional.
/// Es el único punto de acceso externo al grupo de entidades.
/// </summary>
public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
    protected AggregateRoot(TId id) : base(id) { }
}
