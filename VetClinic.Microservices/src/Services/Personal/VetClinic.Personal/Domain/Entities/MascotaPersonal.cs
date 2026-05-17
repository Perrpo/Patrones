namespace VetClinic.Personal.Domain.Entities;
using VetClinic.BuildingBlocks.Domain;
public class MascotaPersonal : Entity<Guid>
{
    public string Nombre { get; private set; }
    public string Especie { get; private set; }
    public string Raza { get; private set; }
    public int Edad { get; private set; }

    public MascotaPersonal(Guid id, string nombre, string especie, string raza, int edad) : base(id)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre obligatorio.");
        Nombre = nombre; Especie = especie; Raza = raza; Edad = edad;
    }
    private MascotaPersonal() : base(Guid.Empty) { Nombre=""; Especie=""; Raza=""; }
}
