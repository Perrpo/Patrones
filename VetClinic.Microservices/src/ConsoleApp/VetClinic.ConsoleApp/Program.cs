using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VetClinic.ConsoleApp;
using VetClinic.ConsoleApp.InMemoryRepos;

// ============ CONFIGURACION DE DEPENDENCIAS (DI) ============
var services = new ServiceCollection();

// Repositorios en memoria (simulan BD aislada por microservicio)
services.AddSingleton<VetClinic.Agendamiento.Domain.Interfaces.IAgendaRepository, InMemoryAgendaRepository>();
services.AddSingleton<VetClinic.Agendamiento.Domain.Interfaces.ICitaRepository, InMemoryCitaRepository>();
services.AddSingleton<VetClinic.Agendamiento.Domain.Interfaces.IMascotaRepository, InMemoryMascotaRepository>();
services.AddSingleton<VetClinic.Facturacion.Domain.Interfaces.IFacturaRepository, InMemoryFacturaRepository>();
services.AddSingleton<VetClinic.Notificaciones.Domain.Interfaces.INotificacionRepository, InMemoryNotificacionRepository>();
services.AddSingleton<VetClinic.Notificaciones.Domain.Interfaces.INotificacionService, ConsoleNotificacionService>();

// Servicios de dominio
services.AddTransient<VetClinic.Agendamiento.Domain.Services.ServicioAgendamiento>();
services.AddTransient<VetClinic.Agendamiento.Domain.Services.ServicioDisponibilidad>();

// Publicador de eventos
services.AddSingleton<VetClinic.BuildingBlocks.Application.IEventPublisher, ConsoleEventPublisher>();

// Use Cases - Agendamiento
services.AddTransient<VetClinic.Agendamiento.Application.UseCases.AgendarCitaUseCase>();
services.AddTransient<VetClinic.Agendamiento.Application.UseCases.ConfirmarCitaUseCase>();
services.AddTransient<VetClinic.Agendamiento.Application.UseCases.CancelarCitaUseCase>();
services.AddTransient<VetClinic.Agendamiento.Application.UseCases.ConsultarAgendaUseCase>();
services.AddTransient<VetClinic.Agendamiento.Application.UseCases.ObtenerCitaUseCase>();
services.AddTransient<VetClinic.Agendamiento.Application.UseCases.ConsultarDisponibilidadUseCase>();

// Use Cases - Facturacion
services.AddTransient<VetClinic.Facturacion.Application.UseCases.RegistrarPagoUseCase>();

// Use Cases - Notificaciones
services.AddTransient<VetClinic.Notificaciones.Application.UseCases.EnviarNotificacionUseCase>();
services.AddTransient<VetClinic.Notificaciones.Application.UseCases.ProcesarPendientesUseCase>();
services.AddTransient<VetClinic.Notificaciones.Application.Consumers.CitaConfirmadaConsumer>();

// Logging
services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Warning));

services.AddTransient<MenuPrincipal>();
var provider = services.BuildServiceProvider();

// Cargar datos iniciales
await DatosSemilla.CargarAsync(provider);

// Ejecutar menu
var menu = provider.GetRequiredService<MenuPrincipal>();
await menu.EjecutarAsync();
