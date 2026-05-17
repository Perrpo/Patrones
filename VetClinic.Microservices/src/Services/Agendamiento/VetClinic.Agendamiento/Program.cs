using Microsoft.EntityFrameworkCore;
using VetClinic.Agendamiento.API.Middleware;
using VetClinic.Agendamiento.Application.UseCases;
using VetClinic.Agendamiento.Domain.Interfaces;
using VetClinic.Agendamiento.Domain.Services;
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
builder.Services.AddScoped<IEventPublisher, InMemoryEventPublisher>();
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

// Middleware global de manejo de errores
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
