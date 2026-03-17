namespace VetClinicConsole.Interfaces;

using VetClinicConsole.Classes.Citas;

public interface IEstadoCita
{
    void Confirmar(Cita cita);
    void Cancelar(Cita cita);
    void Finalizar(Cita cita);
    string Nombre { get; }
}
