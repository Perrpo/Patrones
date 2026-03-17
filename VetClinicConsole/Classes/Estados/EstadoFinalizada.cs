namespace VetClinicConsole.Classes.Estados;

using VetClinicConsole.Classes.Citas;
using VetClinicConsole.Interfaces;

public class EstadoFinalizada : IEstadoCita
{
    public string Nombre => "Finalizada";

    public void Confirmar(Cita cita) { }

    public void Cancelar(Cita cita) { }

    public void Finalizar(Cita cita) { }
}
