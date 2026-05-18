using Microsoft.Extensions.DependencyInjection;
using VetClinic.Agendamiento.Domain.Aggregates;
using VetClinic.Agendamiento.Domain.Entities;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.Agendamiento.Domain.ValueObjects;

namespace VetClinic.ConsoleApp;

/// <summary>Carga datos iniciales en memoria para demostracion.</summary>
public static class DatosSemilla
{
    // IDs fijos para referencia
    public static readonly Guid ProfesionalCarlosId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid ProfesionalMariaId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid MascotaMaxId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid MascotaLunaId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    public static readonly Guid MascotaRockyId = Guid.Parse("55555555-5555-5555-5555-555555555555");
    public static readonly Guid ClienteJuanId = Guid.Parse("66666666-6666-6666-6666-666666666666");

    public static async Task CargarAsync(IServiceProvider sp)
    {
        var agendaRepo = sp.GetRequiredService<IAgendaRepository>();
        var mascotaRepo = sp.GetRequiredService<IMascotaRepository>();

        // Mascotas
        await mascotaRepo.AddAsync(new Mascota(MascotaMaxId, "Max", "Perro", "Golden Retriever", 3, ClienteJuanId));
        await mascotaRepo.AddAsync(new Mascota(MascotaLunaId, "Luna", "Gato", "Siames", 2, ClienteJuanId));
        await mascotaRepo.AddAsync(new Mascota(MascotaRockyId, "Rocky", "Perro", "Bulldog", 5, ClienteJuanId));

        // Agenda Dr. Carlos (veterinario) - Lun a Sab, 8:00-17:00
        var agendaCarlos = new Agenda(Guid.NewGuid(), ProfesionalCarlosId, "Dr. Carlos Rodriguez");
        var hoy = DateTime.Today;
        for (int i = 0; i < 365; i++)
        {
            var dia = hoy.AddDays(i);
            if (dia.DayOfWeek == DayOfWeek.Sunday) continue; // No trabaja domingos
            agendaCarlos.AgregarHorarioLaboral(new HorarioLaboral(
                dia, TimeSpan.FromHours(8), TimeSpan.FromHours(17)));
        }
        await agendaRepo.AddAsync(agendaCarlos);

        // Agenda Dra. Maria (veterinaria) - Lun a Sab, 9:00-18:00
        var agendaMaria = new Agenda(Guid.NewGuid(), ProfesionalMariaId, "Dra. Maria Lopez");
        for (int i = 0; i < 365; i++)
        {
            var dia = hoy.AddDays(i);
            if (dia.DayOfWeek == DayOfWeek.Sunday) continue;
            agendaMaria.AgregarHorarioLaboral(new HorarioLaboral(
                dia, TimeSpan.FromHours(9), TimeSpan.FromHours(18)));
        }
        await agendaRepo.AddAsync(agendaMaria);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  Datos iniciales cargados: 2 profesionales, 3 mascotas, horarios Lun-Sab (1 anio).");
        Console.ResetColor();
    }
}
