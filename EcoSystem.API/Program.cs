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

var app = builder.Build();

// 2. Activamos la interfaz gráfica "boni" de Swagger 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();