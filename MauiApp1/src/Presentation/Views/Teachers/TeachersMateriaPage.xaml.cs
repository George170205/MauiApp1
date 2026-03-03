using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MauiApp1
{
    public partial class TeachersMateriaPage : ContentPage
    {
        public TeachersMateriaPage()
        {
            InitializeComponent();
        }

        // Botón "Generar reporte de asistencias (DO)"
        private async void OnGenerarReporteClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert(
                "Generar Reporte",
                "¿Deseas generar el reporte de asistencias de Desarrollo Organizacional?",
                "Sí, generar",
                "Cancelar");

            if (confirmar)
            {
                // Aquí iría la lógica para generar y descargar/enviar el reporte
                await DisplayAlert("Reporte", "El reporte se ha generado exitosamente.", "OK");
            }
        }

        // Bottom Navigation - Inicio
        private async void OnInicioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeachersPage());
        }

        // Bottom Navigation - Horario
        private async void OnHorarioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeachersHorarioPage());
        }

        // Bottom Navigation - Perfil
        private async void OnPerfilClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeachersPerfilPage());
        }
    }
}
