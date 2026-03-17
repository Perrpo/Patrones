namespace VetClinicConsole.Classes.Personal;

public abstract class Persona
{
    public int Id { get; }
    public string Nombre { get; }
    public string Email { get; }

    protected Persona(int id, string nombre, string email)
    {
        Id = id;
        Nombre = nombre;
        Email = email;
    }
}
