using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;

namespace EcoSystem.Client.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    // Contrato esencial para actualizar la UI en tiempo real
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
            if (_nombreUsuario == value) return; // Guardia de igualdad
            _nombreUsuario = value;
            OnPropertyChanged();
            LoginCommand.NotifyCanExecuteChanged(); // Reevalúa si el botón debe habilitarse
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

    public IRelayCommand LoginCommand { get; }
    public IRelayCommand VerificarCommand { get; }

    public LoginViewModel()
    {
        // El comando de Login evalúa si puede ejecutarse
        LoginCommand = new RelayCommand(EjecutarLogin, PuedeEjecutarLogin);

        // Comando para la demostración de la Firma 3
        VerificarCommand = new RelayCommand(() =>
            Console.WriteLine($"Usuario en ViewModel: {NombreUsuario} | Contraseña: {Contrasena}"));
    }

    private bool PuedeEjecutarLogin() =>
        !string.IsNullOrWhiteSpace(NombreUsuario) && !string.IsNullOrWhiteSpace(Contrasena);

    private void EjecutarLogin()
    {
        Console.WriteLine($"Iniciando sesión para: {NombreUsuario}");
    }
}