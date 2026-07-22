using CommunityToolkit.Mvvm.ComponentModel;
using EcoSystem.Client.Models;

namespace EcoSystem.Client.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = "EcoSystem Connect";

    [ObservableProperty]
    private List<Ecosystem> ecosystems = new();
}