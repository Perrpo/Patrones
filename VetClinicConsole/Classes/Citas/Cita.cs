namespace VetClinicConsole.Classes.Citas;

using VetClinicConsole.Classes.Agendas;
using VetClinicConsole.Classes.Facturacion;
using VetClinicConsole.Classes.Estados;
using VetClinicConsole.Interfaces;

public class Cita
{
    public int Id { get; }
    public HorarioDisponible Horario { get; }
    public IAgendable Profesional { get; }
    public Mascota Mascota { get; }
    public Factura Factura { get; }
    public IEstadoCita Estado { get; private set; }
    public string EstadoNombre => Estado.Nombre;

    public Cita(int id, HorarioDisponible horario, Mascota mascota, IAgendable profesional)
    {
        Id = id;
        Horario = horario;
        Profesional = profesional;
        Mascota = mascota;
        Factura = new Factura(id, this);
        Estado = new EstadoPendiente();
    }

    internal void CambiarEstado(IEstadoCita nuevo) => Estado = nuevo;

    public void Confirmar() => Estado.Confirmar(this);
    public void Cancelar() => Estado.Cancelar(this);
    public void Finalizar() => Estado.Finalizar(this);
}
