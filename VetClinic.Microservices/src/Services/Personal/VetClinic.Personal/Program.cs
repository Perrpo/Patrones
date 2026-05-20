using VetClinic.Personal.Application.UseCases;
using VetClinic.Personal.Domain.Aggregates;
using VetClinic.Personal.Domain.Interfaces;
using VetClinic.Personal.Domain.ValueObjects;
using VetClinic.Personal.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Repositorios (Infrastructure implementa interfaces de Domain — DIP)
builder.Services.AddSingleton<IClienteRepository, InMemoryClienteRepository>();
builder.Services.AddSingleton<IProfesionalRepository, InMemoryProfesionalRepository>();

// Use Cases (Application layer — cada uno con una única acción)
builder.Services.AddScoped<ObtenerClientesUseCase>();
builder.Services.AddScoped<ObtenerClientePorIdUseCase>();
builder.Services.AddScoped<ObtenerProfesionalesUseCase>();
builder.Services.AddScoped<ObtenerProfesionalPorIdUseCase>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── Datos semilla: Clientes con sus mascotas ─────────────────────────────────
var clienteRepo = app.Services.GetRequiredService<IClienteRepository>();

var juan = new Cliente(Guid.Parse("66666666-6666-6666-6666-666666666666"),
    "Juan Pérez", new Email("juan.perez@email.com"));
juan.AgregarMascota("Max",   "Perro", "Golden Retriever", 3);
juan.AgregarMascota("Luna",  "Gato",  "Siamés",           2);
juan.AgregarMascota("Rocky", "Perro", "Bulldog",          5);
await clienteRepo.AddAsync(juan);

var maria = new Cliente(Guid.Parse("77777777-7777-7777-7777-777777777777"),
    "María García", new Email("maria.garcia@email.com"));
maria.AgregarMascota("Mia",   "Gato",  "Persa",            4);
maria.AgregarMascota("Bruno", "Perro", "Labrador",         1);
await clienteRepo.AddAsync(maria);

var carlos = new Cliente(Guid.Parse("88888888-8888-8888-8888-888888888888"),
    "Carlos López", new Email("carlos.lopez@email.com"));
carlos.AgregarMascota("Toby", "Perro", "Beagle",          6);
await clienteRepo.AddAsync(carlos);
// ─────────────────────────────────────────────────────────────────────────────

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
