using Microsoft.EntityFrameworkCore;
using VetClinic.Personal.Application.UseCases;
using VetClinic.Personal.Domain.Interfaces;
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
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
