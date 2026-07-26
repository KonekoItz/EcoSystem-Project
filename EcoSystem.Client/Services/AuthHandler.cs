using System.Net.Http.Headers;
using System.Net;

namespace EcoSystem.Client.Services;

public class AuthHandler : DelegatingHandler
{
    private readonly TokenService _tokenService;

    public AuthHandler(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // 1. Leer el token del almacenamiento
        var token = await _tokenService.GetTokenAsync();

        // 2. Si el token existe, agregarlo a la cabecera
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // 3. Enviar la petición al servidor
        var response = await base.SendAsync(request, cancellationToken);

        // 4. Si el servidor responde con 401 Unauthorized, limpiamos credenciales
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _tokenService.ClearTokenAsync();
            // Aquí idealmente se redirige al login usando NavigationManager
        }

        return response;
    }
}