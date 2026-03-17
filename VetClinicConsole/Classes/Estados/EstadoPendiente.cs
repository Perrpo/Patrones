namespace VetClinicConsole.Classes.Estados;

using VetClinicConsole.Classes.Citas;
using VetClinicConsole.Interfaces;

public class EstadoPendiente : IEstadoCita
{
    public string Nombre => "Pendiente";

    public void Confirmar(Cita cita) => cita.CambiarEstado(new EstadoConfirmada());

    public void Cancelar(Cita cita) => cita.CambiarEstado(new EstadoCancelada());

    public void Finalizar(Cita cita) => cita.CambiarEstado(new EstadoFinalizada());
}
