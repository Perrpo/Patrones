namespace VetClinicConsole.Classes.Estados;

using VetClinicConsole.Classes.Citas;
using VetClinicConsole.Interfaces;

public class EstadoCancelada : IEstadoCita
{
    public string Nombre => "Cancelada";

    public void Confirmar(Cita cita) { }

    public void Cancelar(Cita cita) { }

    public void Finalizar(Cita cita) { }
}
