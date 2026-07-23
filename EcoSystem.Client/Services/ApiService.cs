using System.Net.Http.Json;
using EcoSystem.Client.Models;

namespace EcoSystem.Client.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(HttpClient http)
    {
        _http = http;
        // Se recomienda un timeout para evitar esperas infinitas si la API no responde
        _http.Timeout = TimeSpan.FromSeconds(15);
    }

    public async Task<List<Producto>> GetProductosAsync()
    {
        try
        {
            // Hace la petición GET asíncrona
            var response = await _http.GetAsync("api/Productos");

            // Lanza una excepción si el código HTTP es un error (4xx o 5xx)
            response.EnsureSuccessStatusCode();

            // Deserializa el JSON a una lista de productos en C#
            var productos = await response.Content.ReadFromJsonAsync<List<Producto>>();

            return productos ?? new List<Producto>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error de red: {ex.Message}");
            return new List<Producto>();
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Tiempo de espera agotado.");
            return new List<Producto>();
        }
    }
}