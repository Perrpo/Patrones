namespace VetClinicConsole.Classes.Personal;

public abstract class Empleado : Persona
{
    public string Rol { get; }

    protected Empleado(int id, string nombre, string email, string rol) : base(id, nombre, email)
    {
        Rol = rol;
    }
}
