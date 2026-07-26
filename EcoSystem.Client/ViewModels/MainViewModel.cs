using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using EcoSystem.Client.Services;
using EcoSystem.Client.Models;

namespace EcoSystem.Client.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // Estado local de los productos[cite: 3]
    public ObservableCollection<Producto> Productos { get; set; } = new();

    // Objeto que se conectará al formulario[cite: 3]
    public Producto ProductoActual { get; set; } = new();

    private bool _cargando;
    public bool Cargando
    {
        get => _cargando;
        set { _cargando = value; OnPropertyChanged(); }
    }

    private bool _esEdicion;
    public bool EsEdicion
    {
        get => _esEdicion;
        set { _esEdicion = value; OnPropertyChanged(); }
    }

    public IAsyncRelayCommand CargarProductosCommand { get; }
    public IAsyncRelayCommand GuardarCommand { get; }
    public IAsyncRelayCommand<int> EliminarCommand { get; }
    public IRelayCommand<Producto> PrepararEdicionCommand { get; }

    public MainViewModel(ApiService apiService)
    {
        _apiService = apiService;
        CargarProductosCommand = new AsyncRelayCommand(CargarProductosAsync);
        GuardarCommand = new AsyncRelayCommand(GuardarAsync);
        EliminarCommand = new AsyncRelayCommand<int>(EliminarAsync);
        PrepararEdicionCommand = new RelayCommand<Producto>(PrepararEdicion);
    }

    // Re-fetch completo: trae los datos actualizados del servidor[cite: 3]
    public async Task CargarProductosAsync()
    {
        Cargando = true;
        var lista = await _apiService.GetProductosAsync();
        Productos.Clear();
        foreach (var p in lista) Productos.Add(p);
        Cargando = false;
    }

    private void PrepararEdicion(Producto producto)
    {
        EsEdicion = true;
        // Creamos una copia para no alterar la tabla antes de guardar
        ProductoActual = new Producto
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Precio = producto.Precio,
            Stock = producto.Stock
        };
    }

    private async Task GuardarAsync()
    {
        Cargando = true; // Deshabilita el botón mientras la petición está en vuelo[cite: 3]

        bool exito = EsEdicion
            ? await _apiService.ActualizarProductoAsync(ProductoActual.Id, ProductoActual)
            : await _apiService.CrearProductoAsync(ProductoActual);

        if (exito)
        {
            await CargarProductosAsync(); // Refresca la tabla tras el éxito[cite: 3]
            ProductoActual = new Producto(); // Limpia el formulario[cite: 3]
            EsEdicion = false;
        }
        Cargando = false;
    }

    private async Task EliminarAsync(int id)
    {
        Cargando = true;
        if (await _apiService.EliminarProductoAsync(id))
        {
            await CargarProductosAsync();
        }
        Cargando = false;
    }
}