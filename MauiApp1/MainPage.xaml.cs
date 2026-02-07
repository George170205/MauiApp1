namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnUsuariosClicked(object sender, EventArgs e)
        {
            // Navegar a la página de gestión de usuarios
            // await Navigation.PushAsync(new UsuariosPage());
            await DisplayAlert("Navegación", "Ir a Gestión de Usuarios", "OK");
        }

        private async void OnMateriasClicked(object sender, EventArgs e)
        {
            // Navegar a la página de gestión de materias
            // await Navigation.PushAsync(new MateriasPage());
            await DisplayAlert("Navegación", "Ir a Gestión de Materias", "OK");
        }

        private async void OnGruposClicked(object sender, EventArgs e)
        {
            // Navegar a la página de gestión de grupos
            // await Navigation.PushAsync(new GruposPage());
            await DisplayAlert("Navegación", "Ir a Gestión de Grupos", "OK");
        }

        private async void OnReportesClicked(object sender, EventArgs e)
        {
            // Navegar a la página de reportes
            // await Navigation.PushAsync(new ReportesPage());
            await DisplayAlert("Navegación", "Ir a Reportes", "OK");
        }

        private async void OnVerAlertasClicked(object sender, EventArgs e)
        {
            // Navegar a la página de alertas
            // await Navigation.PushAsync(new AlertasPage());
            await DisplayAlert("Navegación", "Ver todas las alertas", "OK");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Aquí puedes cargar datos desde tu API o base de datos
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            // TODO: Implementar carga de datos desde el backend
            // Ejemplo:
            // var stats = await _adminService.GetStatisticsAsync();
            // UpdateUI(stats);
        }
    }
}
