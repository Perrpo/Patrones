namespace VetClinicConsole.Classes.Patrones;

using System.Threading;
using VetClinicConsole.Classes.Agendas;
using VetClinicConsole.Classes.Citas;
using VetClinicConsole.Interfaces;

public class CitaFactory
{
    private int _citaCounter = 0;

    public Cita CrearCita(HorarioDisponible horario, Mascota mascota, IAgendable profesional, IEnumerable<IServicio> servicios)
    {
        var id = Interlocked.Increment(ref _citaCounter);
        var cita = new Cita(id, horario, mascota, profesional);
        cita.Factura.AgregarServicios(servicios);
        profesional.ObtenerAgenda().AgregarCita(cita);
        return cita;
    }
}
