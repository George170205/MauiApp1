using MauiApp1.src.Presentation.ViewModels.Student;

namespace MauiApp1.src.Presentation.Views
{
    public partial class PerfilAlumnoPage : ContentPage
    {
        private PerfilAlumnoViewModel _vm = null!;

        public PerfilAlumnoPage()
        {
            InitializeComponent();

            _vm = new PerfilAlumnoViewModel(async () =>
            {
                await Shell.Current.GoToAsync("//Login");
            });
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
}
