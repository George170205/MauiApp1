using MauiApp1.src.Presentation.ViewModels.Student;

namespace MauiApp1.src.Presentation.Views
{
    public partial class HorarioAlumnoPage : ContentPage
    {
        private HorarioAlumnoViewModel _vm = null!;

        public HorarioAlumnoPage()
        {
            InitializeComponent();

            _vm = new HorarioAlumnoViewModel();
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
