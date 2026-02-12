namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnConfiguracionClicked(object sender, EventArgs e)
        {
            var opcion = await DisplayActionSheet(
                "Configuración",
                "Cancelar",
                null,
                "✏️ Editar Perfil",
                "🔔 Notificaciones",
                "🌐 Idioma",
                "🚪 Cerrar Sesión");

            switch (opcion)
            {
                case "✏️ Editar Perfil":
                    await DisplayAlert("Perfil", "Editar perfil", "OK");
                    break;
                case "🔔 Notificaciones":
                    await DisplayAlert("Notificaciones", "Configurar notificaciones", "OK");
                    break;
                case "🌐 Idioma":
                    OnIdiomaClicked(sender, e);
                    break;
                case "🚪 Cerrar Sesión":
                    // Lógica de cerrar sesión
                    // Confirmar antes de cerrar sesión
                    bool confirmar = await DisplayAlert(
                        "Cerrar Sesión",
                        "¿Estás seguro que deseas cerrar sesión?",
                        "Sí",
                        "No");

                    if (confirmar)
                    {
                        // Limpiar las preferencias guardadas
                        Preferences.Remove("recordar_sesion");
                        Preferences.Remove("usuario");
                        Preferences.Remove("rol");

                        // Regresar al LoginPage
                        Application.Current.MainPage = new LoginPage();
                    }

                    break;
            }
        }
        private async void OnIdiomaClicked(object sender, EventArgs e)
        {
            var idioma = await DisplayActionSheet(
                "Seleccionar Idioma",
                "Cancelar",
                null,
                "🇲🇽 Español",
                "🇺🇸 English");

            if (idioma != null && idioma != "Cancelar")
            {
                await DisplayAlert("Idioma", $"Idioma seleccionado: {idioma}", "OK");
            }
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
