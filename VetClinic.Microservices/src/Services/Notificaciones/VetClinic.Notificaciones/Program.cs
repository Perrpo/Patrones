using Microsoft.EntityFrameworkCore;
using VetClinic.Notificaciones.Application.Consumers;
using VetClinic.Notificaciones.Application.UseCases;
using VetClinic.Notificaciones.Domain.Interfaces;
using VetClinic.Notificaciones.Infrastructure.Persistence;
using VetClinic.Notificaciones.Infrastructure.Repositories;
using VetClinic.Notificaciones.Infrastructure.Services;
using VetClinic.Notificaciones.Worker;

var builder = Host.CreateApplicationBuilder(args);

// ORM InMemory
builder.Services.AddDbContext<NotificacionesDbContext>(o =>
    o.UseInMemoryDatabase("NotificacionesDB"));

// Repositorios
builder.Services.AddScoped<INotificacionRepository, NotificacionRepository>();

// Servicios de infraestructura
builder.Services.AddScoped<INotificacionService, EmailNotificationService>();

// Use Cases
builder.Services.AddScoped<EnviarNotificacionUseCase>();
builder.Services.AddScoped<ProcesarPendientesUseCase>();

// Consumers
builder.Services.AddScoped<CitaConfirmadaConsumer>();
builder.Services.AddScoped<PagoRegistradoConsumer>();

// Worker (BackgroundService)
builder.Services.AddHostedService<NotificacionWorker>();

var host = builder.Build();
host.Run();
