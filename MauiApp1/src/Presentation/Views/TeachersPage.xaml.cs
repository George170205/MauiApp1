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

                    QRGrid.Add(box, col, row);
                }
            }
        }

        // Event Handler para el botón de configuración
        private async void OnConfiguracionClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Configuración", "Abriendo configuración...", "OK");
            // Aquí navegarías a la página de configuración
            // await Navigation.PushAsync(new ConfiguracionPage());
        }

        // Event Handlers para los botones del menú principal
        private async void OnMisGruposTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Mis Grupos", "Navegando a Mis Grupos...\n\n5 grupos activos", "OK");
            // await Navigation.PushAsync(new GruposPage());
        }

        private async void OnAsistenciasTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Asistencias", "Navegando a Asistencias...\n\n156 registros totales", "OK");
            // await Navigation.PushAsync(new AsistenciasPage());
        }

        private async void OnCalificacionesTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Calificaciones", "Navegando a Calificaciones...\n\nPromedio general: 8.5", "OK");
            // await Navigation.PushAsync(new CalificacionesPage());
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

        // Event Handler para ver detalles de la clase
        private async void OnVerClaseClicked(object sender, EventArgs e)
        {
            await DisplayAlert(
                "Detalles de la Clase",
                "Grupo 5A - Álgebra Lineal\nHoy 10:30 AM\nAula 205\n32 alumnos\n\nTema: Sistemas de ecuaciones lineales",
                "OK"
            );
            // await Navigation.PushAsync(new DetallesClasePage());
        }

        // Event Handler para ver actividades
        private async void OnVerActividadesClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Actividad Reciente", "Mostrando historial completo de actividades...", "OK");
            // await Navigation.PushAsync(new ActividadesPage());
        }

        // Event Handler para ver alertas
        private async void OnVerAlertasClicked(object sender, EventArgs e)
        {
            await DisplayAlert(
                "Alertas Pendientes",
                "• 5 alumnos con baja asistencia (<75%)\n• 3 calificaciones pendientes de capturar\n\nTotal: 2 alertas activas",
                "OK"
            );
            // await Navigation.PushAsync(new AlertasPage());
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
