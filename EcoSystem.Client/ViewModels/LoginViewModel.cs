using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using EcoSystem.Client.Services;
using Microsoft.AspNetCore.Components;

namespace EcoSystem.Client.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private readonly TokenService _tokenService;
    private readonly NavigationManager _navigationManager;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private string _nombreUsuario = string.Empty;
    public string NombreUsuario
    {
        get => _nombreUsuario;
        set
        {
            if (_nombreUsuario == value) return;
            _nombreUsuario = value;
            OnPropertyChanged();
            LoginCommand.NotifyCanExecuteChanged();
        }
    }

    private string _contrasena = string.Empty;
    public string Contrasena
    {
        get => _contrasena;
        set
        {
            if (_contrasena == value) return;
            _contrasena = value;
            OnPropertyChanged();
            LoginCommand.NotifyCanExecuteChanged();
        }
    }

    public IAsyncRelayCommand LoginCommand { get; }
    public IRelayCommand VerificarCommand { get; }

    // Inyectamos los servicios de API, Token y Navegación
    public LoginViewModel(ApiService apiService, TokenService tokenService, NavigationManager navigationManager)
    {
        _apiService = apiService;
        _tokenService = tokenService;
        _navigationManager = navigationManager;

        LoginCommand = new AsyncRelayCommand(EjecutarLoginAsync, PuedeEjecutarLogin);
        VerificarCommand = new RelayCommand(() =>
            Console.WriteLine($"Usuario: {NombreUsuario} | Pass: {Contrasena}"));
    }

    private bool PuedeEjecutarLogin() =>
        !string.IsNullOrWhiteSpace(NombreUsuario) && !string.IsNullOrWhiteSpace(Contrasena);

    private async Task EjecutarLoginAsync()
    {
        Console.WriteLine($"Conectando con el servidor para autenticar a {NombreUsuario}...");

        // Ejecutamos la petición POST[cite: 2]
        var result = await _apiService.LoginAsync(NombreUsuario, Contrasena);

        if (result != null && !string.IsNullOrEmpty(result.Token))
        {
            // Guardamos el JWT en el SecureStorage del cliente[cite: 2]
            await _tokenService.SaveTokenAsync(result.Token, result.Expiration);
            Console.WriteLine("¡Login exitoso! Token cifrado y guardado. Redirigiendo...");

            // Redirigimos al área protegida[cite: 2]
            _navigationManager.NavigateTo("/");
        }
        else
        {
            Console.WriteLine("Error: Credenciales incorrectas o servidor no disponible.");
        }
    }
}