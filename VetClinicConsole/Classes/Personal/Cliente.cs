namespace VetClinicConsole.Classes.Personal;

public class Cliente : Persona
{
    public Cliente(int id, string nombre, string email) : base(id, nombre, email)
    {
    }

    public bool ValidarDatos() =>
        !string.IsNullOrWhiteSpace(Nombre) &&
        !string.IsNullOrWhiteSpace(Email) &&
        Email.Contains("@");
}
