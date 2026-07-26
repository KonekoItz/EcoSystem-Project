using System.Net.Http.Json;
using EcoSystem.Client.Models;

namespace EcoSystem.Client.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(HttpClient http)
    {
        _http = http;
        _http.Timeout = TimeSpan.FromSeconds(15);
    }

    public async Task<List<Producto>> GetProductosAsync()
    {
        try
        {
            var response = await _http.GetAsync("api/Productos");
            response.EnsureSuccessStatusCode();
            var productos = await response.Content.ReadFromJsonAsync<List<Producto>>();
            return productos ?? new List<Producto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de red: {ex.Message}");
            return new List<Producto>();
        }
    }

    // Nuevo método para enviar las credenciales por POST
    public async Task<AuthResponse?> LoginAsync(string username, string password)
    {
        try
        {
            var payload = new { Username = username, Password = password };
            var response = await _http.PostAsJsonAsync("api/auth/login", payload);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthResponse>();
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
            return null;
        }
    }
}