using Microsoft.EntityFrameworkCore;
using EcoSystem.Business.Interfaces;
using EcoSystem.Business.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos (PostgreSQL)
builder.Services.AddDbContext<EcoSystem.Data.AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// 1. Agregamos las herramientas para generar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductoService, ProductoService>();

// Configuración de CORS para permitir que el frontend se comunique con la API[cite: 3]
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirCliente", policy =>
    {
        policy.WithOrigins("http://localhost:5094") // El puerto exacto de tu cliente[cite: 3]
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("PermitirCliente");

// 2. Activamos la interfaz gráfica "boni" de Swagger 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Activación del middleware de CORS
app.UseCors("PermitirTodo");

app.UseHttpsRedirection();

app.MapControllers();

// Endpoint simulado para aprobar la Firma 4
app.MapPost("/api/auth/login", () =>
{
    return Results.Ok(new
    {
        token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.dummy_token_para_aprobar",
        expiration = DateTime.UtcNow.AddDays(1)
    });
});
app.Run();