namespace VetClinicConsole.Classes.Agendas;

using VetClinicConsole.Classes.Personal;

public class Mascota
{
    public int Id { get; }
    public string Nombre { get; }
    public string Especie { get; }
    public string Raza { get; }
    public int Edad { get; }
    public Cliente Cliente { get; }

    public Mascota(int id, string nombre, string especie, string raza, int edad, Cliente cliente)
    {
        Id = id;
        Nombre = nombre;
        Especie = especie;
        Raza = raza;
        Edad = edad;
        Cliente = cliente;
    }
}
