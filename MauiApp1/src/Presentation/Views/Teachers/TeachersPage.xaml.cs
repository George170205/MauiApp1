using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class TeachersPage : ContentPage
    {
        public TeachersPage()
        {
            InitializeComponent();
            GenerateQRPattern();
        }

        /// <summary>
        /// Genera un patrón visual simple para simular un código QR
        /// </summary>
        private void GenerateQRPattern()
        {
            // Patrón básico de QR code (simplificado para demostración)
            // 1 = negro, 0 = blanco
            int[,] qrPattern = new int[8, 8]
            {
                { 1, 1, 1, 1, 1, 0, 1, 1 },
                { 1, 0, 0, 0, 1, 0, 1, 0 },
                { 1, 0, 1, 0, 1, 1, 0, 1 },
                { 1, 0, 0, 0, 1, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 0, 1, 1 },
                { 0, 0, 1, 0, 0, 1, 0, 1 },
                { 1, 1, 0, 1, 0, 1, 1, 0 },
                { 1, 0, 1, 0, 1, 0, 1, 1 }
            };

            // Generar los cuadros del QR
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var box = new BoxView
                    {
                        Color = qrPattern[row, col] == 1 ? Colors.Black : Colors.White
                    };

                    var grid = this.FindByName<Microsoft.Maui.Controls.Grid>("QRGrid");
                    if (grid != null)
                    {
                        grid.Add(box, col, row);
                    }
                }

            }
        }

        // Handler para evento Tapped/Clicked asociado a "Mis Grupos" desde XAML
        // Se añaden dos sobrecargas por compatibilidad con diferentes firmas de evento (TappedEventArgs o EventArgs)
        private async void OnMisGruposTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Mis Grupos", "Navegando a Mis Grupos...", "OK");
            // await Navigation.PushAsync(new MisGruposPage()); // Descomentar si existe la página
        }

        private async void OnMisGruposTapped(object sender, Microsoft.Maui.Controls.TappedEventArgs e)
        {
            await DisplayAlert("Mis Grupos", "Navegando a Mis Grupos...", "OK");
            // await Navigation.PushAsync(new MisGruposPage()); // Descomentar si existe la página
        }

        // Event Handler para el botón de configuración
        private async void OnConfiguracionClicked(object sender, EventArgs e)
        {
            string accion = await DisplayActionSheet("Configuración", "Cancelar", null,
                "Editar perfil", "Cambiar contraseña", "Cerrar sesión");

            if (accion == "Cerrar sesión")
            {
                bool confirmar = await DisplayAlert("Cerrar sesión", "¿Estás seguro de que deseas cerrar sesión?", "Sí", "No");
                if (confirmar)
                {
                    // Navegar al login
                    Application.Current!.MainPage = new NavigationPage(new MainPage());
                }
            }
        }

        // Botón "Ver Clase" en la tarjeta de próxima clase
        private async void OnVerClaseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeachersMateriaPage());
        }

        // Botón "Ver Todas las Actividades"
        private async void OnVerActividadesClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Actividades", "Aquí se mostrarían todas las actividades recientes.", "Cerrar");
        }

        // Botón "Ver Todas las Alertas"
        private async void OnVerAlertasClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Alertas Pendientes", "Revisa las alertas en el panel de administración.", "Cerrar");
        }

        // Navegación inferior: Inicio (ya estamos aquí, sin acción)
        private void OnInicioClicked(object sender, EventArgs e)
        {
            // Ya estamos en la página principal
        }

        // Navegación inferior: Horario
        private async void OnHorarioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeachersHorarioPage());
        }

        // Navegación inferior: Perfil
        private async void OnPerfilClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeachersPerfilPage());
        }
        // Event Handler para regenerar el código QR
        private async void OnRegenerarQRClicked(object sender, EventArgs e)
        {
            bool respuesta = await DisplayAlert(
                "Regenerar QR",
                "¿Desea generar un nuevo código QR de asistencia?\n\nEl código actual dejará de funcionar.",
                "Sí",
                "No"
            );

            if (respuesta)
            {
                // Aquí iría la lógica para regenerar el QR
                await DisplayAlert("QR Regenerado", "Nuevo código QR generado exitosamente", "OK");
                // Podrías llamar a GenerateQRPattern() de nuevo con un nuevo patrón
            }
        }

        // Event Handlers adicionales si necesitas agregar más funcionalidad
        private async void OnReportesTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Reportes", "Generando reportes académicos...", "OK");
            // await Navigation.PushAsync(new ReportesPage());
        }

        // Event Handlers para la barra de navegación inferior (si decides agregarla)
        private async void OnCalendarioTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Calendario", "Navegando a Calendario académico...", "OK");
        }

        private async void OnMensajesTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Mensajes", "Tienes 3 mensajes nuevos", "OK");
        }

        private async void OnMasTapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet(
                "Más opciones",
                "Cancelar",
                null,
                "Mi Perfil",
                "Configuración",
                "Ayuda",
                "Cerrar Sesión"
            );

            if (action != "Cancelar" && action != null)
            {
                await DisplayAlert("Selección", $"Has seleccionado: {action}", "OK");
            }
        }
    }
}
