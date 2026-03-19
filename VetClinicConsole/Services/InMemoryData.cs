namespace VetClinicConsole.Services;

using VetClinicConsole.Classes.Agendas;
using VetClinicConsole.Classes.Citas;
using VetClinicConsole.Classes.Pagos;
using VetClinicConsole.Classes.Patrones;
using VetClinicConsole.Classes.Personal;
using VetClinicConsole.Classes.Servicios;
using VetClinicConsole.Interfaces;

public class InMemoryData
{
    public required List<Cliente> Clientes { get; init; }
    public required List<Mascota> Mascotas { get; init; }
    public required List<IAgendable> Profesionales { get; init; }
    public required List<IServicio> Servicios { get; init; }
    public required List<Cita> Citas { get; init; }
    public required List<Pago> Pagos { get; init; }
    public required CitaFactory Factory { get; init; }
    public required AgendamientoFacade Facade { get; init; }

    public static InMemoryData Load()
    {
        static void AgregarHorarioSemanal(IAgendable profesional, int baseId)
        {
            var horarios = Enumerable.Range(0, 7)
                .Select(i => new HorarioLaboral(
                    baseId + i,
                    DateTime.Today.AddDays(i),
                    TimeSpan.FromHours(8),
                    TimeSpan.FromHours(18)));

            profesional.ObtenerAgenda().AgregarHorariosLaborales(horarios);
        }

        var clientes = new List<Cliente>
        {
            new Cliente(1, "María González", "maria@example.com"),
            new Cliente(2, "Carlos Rodríguez", "carlos@example.com"),
            new Cliente(3, "Ana López", "ana@example.com")
        };

        var mascotas = new List<Mascota>
        {
            new Mascota(1, "Luna", "Perro", "Golden Retriever", 3, clientes[0]),
            new Mascota(2, "Miau", "Gato", "Persa", 2, clientes[0]),
            new Mascota(3, "Max", "Perro", "Pastor Alemán", 5, clientes[1])
        };

        var vetCirugia = new Veterinario(1, "Dra. Elena Martín", "elena@vet.com", "cirugia");
        var vetGeneral = new Veterinario(2, "Dr. Mateo Gómez", "mateo@vet.com", "medicina_general");
        var vetUrgencias = new Veterinario(3, "Dra. Sofía Ávila", "sofia@vet.com", "urgencias");

        var peluquero = new Peluquero(4, "Pedro Cortés", "pedro@vet.com");
        var peluquero2 = new Peluquero(5, "Laura Ríos", "laura@vet.com");

        var asistente = new Asistente(6, "Lucía Rivera", "lucia@vet.com", "Recepción");
        var asistente2 = new Asistente(7, "Diego Pérez", "diego@vet.com", "Recepción");

        AgregarHorarioSemanal(vetCirugia, 100);
        AgregarHorarioSemanal(vetGeneral, 200);
        AgregarHorarioSemanal(vetUrgencias, 300);
        AgregarHorarioSemanal(peluquero, 400);
        AgregarHorarioSemanal(peluquero2, 500);
        AgregarHorarioSemanal(asistente, 600);
        AgregarHorarioSemanal(asistente2, 700);

        var profesionales = new List<IAgendable> { vetCirugia, vetGeneral, vetUrgencias, peluquero, peluquero2 };

        var servicios = new List<IServicio>
        {
            new Consulta(),
            new Vacunacion(),
            new CirugiaMenor(),
            new Peluqueria(),
            new Radiografia(),
            new AnalisisSangre(),
            new LimpiezaDental(),
            new Urgencia()
        };

        var citas = new List<Cita>();
        var factory = new CitaFactory();
        var facade = new AgendamientoFacade(citas, factory);

        var cita1 = factory.CrearCita(
            new HorarioDisponible(DateTime.Today.AddDays(1), TimeSpan.FromHours(9), TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(servicios[0].Duracion()))),
            mascotas[0],
            vetGeneral,
            new[] { servicios[0] });
        cita1.Confirmar();

        var cita2 = factory.CrearCita(
            new HorarioDisponible(DateTime.Today.AddDays(2), TimeSpan.FromHours(11), TimeSpan.FromHours(11).Add(TimeSpan.FromMinutes(servicios[3].Duracion()))),
            mascotas[1],
            peluquero,
            new IServicio[] { servicios[3] });

        citas.AddRange(new[] { cita1, cita2 });

        var pagos = new List<Pago>();
        var pago1 = new Pago(1, cita1.Factura.CalcularTotal(), new PagoTarjeta());
        pago1.Procesar();
        cita1.Factura.RegistrarPago(pago1);
        pagos.Add(pago1);

        return new InMemoryData
        {
            Clientes = clientes,
            Mascotas = mascotas,
            Profesionales = profesionales,
            Servicios = servicios,
            Citas = citas,
            Pagos = pagos,
            Factory = factory,
            Facade = facade
        };
    }
}
