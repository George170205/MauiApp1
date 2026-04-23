namespace MauiApp1.src.Presentation.Views;

using MauiApp1.src.Presentation.ViewModels.Student;

public partial class HomeAlumnoPage : ContentPage
{
    private HomeAlumnoViewModel _vm = null!;

    public HomeAlumnoPage()
    {
        InitializeComponent();

        _vm = new HomeAlumnoViewModel();
        _vm.SesionExpirada += OnSesionExpirada;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.CargarDesdeApiAsync();
    }

    private async void OnSesionExpirada(object? sender, EventArgs e)
    {
        await DisplayAlert("Sesión expirada",
            "Tu sesión ha vencido. Inicia sesión de nuevo.", "OK");
        Preferences.Clear();
        await Shell.Current.GoToAsync("//Login");
    }
}
