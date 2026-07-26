using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EcoSystem.Client;
using EcoSystem.Client.Services;
using EcoSystem.Client.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. Configuramos el HttpClient para tu ApiService con la URL local
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5099/");
});

// Pega la línea justo aquí:
builder.Services.AddTransient<LoginViewModel>();

var app = builder.Build();

// 2. Ejecutamos la prueba para la consola (Criterio de Firma 2)
Console.WriteLine("=== EcoSystem Connect Fase 2 ===");
Console.WriteLine("Iniciando petición GET asíncrona...");

var apiService = app.Services.GetRequiredService<ApiService>();
var productos = await apiService.GetProductosAsync();

if (productos.Count == 0)
{
    Console.WriteLine("No se recuperaron productos.");
}
else
{
    Console.WriteLine($"\n{productos.Count} producto(s) recuperado(s) exitosamente:\n");
    foreach (var p in productos)
    {
        Console.WriteLine($" [{p.Id}] {p.Nombre} - ${p.Precio:F2} (Stock: {p.Stock})");
    }
    Console.WriteLine("\nFirma 2. Criterio cumplido ✓");
}

await app.RunAsync();