namespace VetClinic.Personal.Domain.Aggregates;
using VetClinic.BuildingBlocks.Domain;
using VetClinic.Personal.Domain.Entities;
using VetClinic.Personal.Domain.ValueObjects;
/// <summary>AGGREGATE ROOT: Cliente agrupa sus mascotas. Unico punto de acceso.</summary>
public class Cliente : AggregateRoot<Guid>
{
    public string Nombre { get; private set; }
    public Email Email { get; private set; }
    private readonly List<MascotaPersonal> _mascotas = new();
    public IReadOnlyList<MascotaPersonal> Mascotas => _mascotas.AsReadOnly();

    public Cliente(Guid id, string nombre, Email email) : base(id)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("El nombre es obligatorio.");
        Nombre = nombre; Email = email;
    }

    /// <summary>Agrega mascota a traves de la raiz del agregado.</summary>
    public MascotaPersonal AgregarMascota(string nombre, string especie, string raza, int edad)
    {
        var mascota = new MascotaPersonal(Guid.NewGuid(), nombre, especie, raza, edad);
        _mascotas.Add(mascota);
        return mascota;
    }

    public void ActualizarDatos(string nombre, Email email)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("El nombre es obligatorio.");
        Nombre = nombre; Email = email;
    }

    private Cliente() : base(Guid.Empty) { Nombre=""; Email=null!; }
}
