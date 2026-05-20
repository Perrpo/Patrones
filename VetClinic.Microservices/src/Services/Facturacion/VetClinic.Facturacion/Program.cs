using Microsoft.EntityFrameworkCore;
using VetClinic.Facturacion.Application.UseCases;
using VetClinic.Facturacion.Domain.Interfaces;
using VetClinic.Facturacion.Infrastructure.Persistence;
using VetClinic.Facturacion.Infrastructure.Repositories;
using VetClinic.Facturacion.Infrastructure.Services;
using VetClinic.BuildingBlocks.Application;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FacturacionDbContext>(o => o.UseInMemoryDatabase("FacturacionDB"));
builder.Services.AddScoped<IFacturaRepository, FacturaRepository>();
builder.Services.AddScoped<IEventPublisher, VetClinic.Facturacion.Infrastructure.Services.InMemoryEventPublisher>();
builder.Services.AddScoped<RegistrarPagoUseCase>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger(); app.UseSwaggerUI();
app.MapControllers(); app.Run();
