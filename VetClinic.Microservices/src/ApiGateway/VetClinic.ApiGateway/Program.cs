using VetClinic.ApiGateway.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar HttpClient para cada microservicio
builder.Services.AddHttpClient("Agendamiento", c => c.BaseAddress = new Uri("http://localhost:5001"));
builder.Services.AddHttpClient("Facturacion", c => c.BaseAddress = new Uri("http://localhost:5002"));
builder.Services.AddHttpClient("Personal", c => c.BaseAddress = new Uri("http://localhost:5003"));

var app = builder.Build();

// Middleware global
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
