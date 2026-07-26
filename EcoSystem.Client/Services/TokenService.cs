using Microsoft.JSInterop;

namespace EcoSystem.Client.Services;

public class TokenService
{
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "jwt_token";
    private const string ExpiryKey = "jwt_exp";

    public TokenService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    // Persiste el JWT y su expiración en el almacenamiento de sesión
    public async Task SaveTokenAsync(string token, DateTime expiration)
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", TokenKey, token);
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", ExpiryKey, expiration.ToString("O"));
    }

    // Recupera el token validando que no haya expirado localmente
    public async Task<string?> GetTokenAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", TokenKey);
        var expStr = await _jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", ExpiryKey);

        if (string.IsNullOrEmpty(token)) return null;

        if (DateTime.TryParse(expStr, out var expDate))
        {
            // Verifica la expiración agregando un margen de 60 segundos (clock skew)[cite: 3]
            if (expDate > DateTime.UtcNow.AddSeconds(60))
            {
                return token;
            }
        }

        // Si expiró, limpia el rastro por seguridad[cite: 3]
        await ClearTokenAsync();
        return null;
    }

    // Elimina el token (equivalente a cerrar sesión)
    public async Task ClearTokenAsync()
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", TokenKey);
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", ExpiryKey);
    }
}