namespace VetClinicConsole.Classes.Estados;

using VetClinicConsole.Classes.Citas;
using VetClinicConsole.Interfaces;

public class EstadoConfirmada : IEstadoCita
{
    public string Nombre => "Confirmada";

    public void Confirmar(Cita cita) { }

    public void Cancelar(Cita cita) => cita.CambiarEstado(new EstadoCancelada());

    public void Finalizar(Cita cita) => cita.CambiarEstado(new EstadoFinalizada());
}
