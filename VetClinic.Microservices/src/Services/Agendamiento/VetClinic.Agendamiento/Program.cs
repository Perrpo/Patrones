using Microsoft.EntityFrameworkCore;
using VetClinic.Agendamiento.API.Middleware;
using VetClinic.Agendamiento.Application.UseCases;
using VetClinic.Agendamiento.Domain.Aggregates;
using VetClinic.Agendamiento.Domain.Entities;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.Agendamiento.Domain.Services;
using VetClinic.Agendamiento.Domain.ValueObjects;
using VetClinic.Agendamiento.Infrastructure.Cache;
using VetClinic.Agendamiento.Infrastructure.Persistence;
using VetClinic.Agendamiento.Infrastructure.Repositories;
using VetClinic.Agendamiento.Infrastructure.Services;
using VetClinic.BuildingBlocks.Application;

var builder = WebApplication.CreateBuilder(args);

// ORM: Entity Framework Core con proveedor InMemory
builder.Services.AddDbContext<AgendamientoDbContext>(options =>
    options.UseInMemoryDatabase("AgendamientoDB"));

// Cache en memoria
builder.Services.AddMemoryCache();

// Repositorios (Infrastructure implementa interfaces de Domain)
builder.Services.AddScoped<IAgendaRepository, AgendaRepository>();
builder.Services.AddScoped<ICitaRepository, CitaRepository>();
builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();

// Servicios de dominio
builder.Services.AddScoped<ServicioAgendamiento>();

// Servicios de infraestructura
builder.Services.AddScoped<IEventPublisher, VetClinic.Agendamiento.Infrastructure.Services.InMemoryEventPublisher>();
builder.Services.AddScoped<DisponibilidadCacheService>();

// Use Cases (Application layer)
builder.Services.AddScoped<AgendarCitaUseCase>();
builder.Services.AddScoped<ConfirmarCitaUseCase>();
builder.Services.AddScoped<CancelarCitaUseCase>();
builder.Services.AddScoped<ConsultarAgendaUseCase>();
builder.Services.AddScoped<ObtenerCitaUseCase>();
builder.Services.AddScoped<ConsultarDisponibilidadUseCase>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── Datos semilla ────────────────────────────────────────────────────────────
// IDs que coinciden con los del proyecto de consola original
var profesionalCarlosId = Guid.Parse("11111111-1111-1111-1111-111111111111");
var profesionalMariaId  = Guid.Parse("22222222-2222-2222-2222-222222222222");
var clienteJuanId       = Guid.Parse("66666666-6666-6666-6666-666666666666");

using (var scope = app.Services.CreateScope())
{
    var mascotaRepo = scope.ServiceProvider.GetRequiredService<IMascotaRepository>();
    var agendaRepo  = scope.ServiceProvider.GetRequiredService<IAgendaRepository>();

    // Mascotas de Juan (3 mascotas, mismo ID que el ConsoleApp)
    await mascotaRepo.AddAsync(new Mascota(
        Guid.Parse("33333333-3333-3333-3333-333333333333"),
        "Max", "Perro", "Golden Retriever", 3, clienteJuanId));
    await mascotaRepo.AddAsync(new Mascota(
        Guid.Parse("44444444-4444-4444-4444-444444444444"),
        "Luna", "Gato", "Siamés", 2, clienteJuanId));
    await mascotaRepo.AddAsync(new Mascota(
        Guid.Parse("55555555-5555-5555-5555-555555555555"),
        "Rocky", "Perro", "Bulldog", 5, clienteJuanId));

    var hoy = DateTime.Today;

    // Agenda Dr. Carlos (Veterinario) — Lun a Sáb, 08:00-17:00, 1 año
    var agendaCarlos = new Agenda(Guid.NewGuid(), profesionalCarlosId, "Dr. Carlos Rodriguez");
    for (int i = 0; i < 365; i++)
    {
        var dia = hoy.AddDays(i);
        if (dia.DayOfWeek == DayOfWeek.Sunday) continue;
        agendaCarlos.AgregarHorarioLaboral(new HorarioLaboral(
            dia, TimeSpan.FromHours(8), TimeSpan.FromHours(17)));
    }
    await agendaRepo.AddAsync(agendaCarlos);

    // Agenda Dra. María (Veterinaria) — Lun a Sáb, 09:00-18:00, 1 año
    var agendaMaria = new Agenda(Guid.NewGuid(), profesionalMariaId, "Dra. María López");
    for (int i = 0; i < 365; i++)
    {
        var dia = hoy.AddDays(i);
        if (dia.DayOfWeek == DayOfWeek.Sunday) continue;
        agendaMaria.AgregarHorarioLaboral(new HorarioLaboral(
            dia, TimeSpan.FromHours(9), TimeSpan.FromHours(18)));
    }
    await agendaRepo.AddAsync(agendaMaria);
}
// ─────────────────────────────────────────────────────────────────────────────

// Middleware global de manejo de errores
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();

