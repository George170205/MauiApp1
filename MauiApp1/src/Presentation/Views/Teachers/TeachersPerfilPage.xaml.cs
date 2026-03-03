using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public partial class TeachersPerfilPage : ContentPage
    {
        public TeachersPerfilPage()
        {
            InitializeComponent();
        }

        // Solicitud de reportes: Reporte del día de hoy
        private async void OnReporteDiaClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert(
                "Reporte Diario",
                "¿Generar el reporte de asistencias del día de hoy?",
                "Generar", "Cancelar");

            if (confirmar)
                await DisplayAlert("Reporte", "Reporte diario generado correctamente.", "Aceptar");
        }

        // Solicitud de reportes: Reporte semanal
        private async void OnReporteSemanalClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert(
                "Reporte Semanal",
                "¿Generar el reporte semanal de asistencias?",
                "Generar", "Cancelar");

            if (confirmar)
                await DisplayAlert("Reporte", "Reporte semanal generado correctamente.", "Aceptar");
        }

        // Solicitud de reportes: Reporte final
        private async void OnReporteFinalClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert(
                "Reporte Final",
                "¿Generar el reporte final de asistencias? Esta acción no se puede deshacer.",
                "Generar", "Cancelar");

            if (confirmar)
                await DisplayAlert("Reporte", "Reporte final generado correctamente.", "Aceptar");
        }

        // Ajustes: Materias y horarios
        private async void OnMateriasHorariosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeachersHorarioPage());
        }

        // Ajustes: Tiempo de generación de QR
        private async void OnQRClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Configuración QR", "Aquí podrías configurar el tiempo de expiración del código QR.", "Cerrar");
        }

        // Ajustes: Notificaciones
        private async void OnNotificacionesClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Notificaciones", "Aquí podrías gestionar tus preferencias de notificaciones.", "Cerrar");
        }

        // Ajustes: Preferencias del sistema
        private async void OnPreferenciasClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Preferencias del sistema", "Aquí podrías ajustar las preferencias generales del sistema.", "Cerrar");
        }

        // Solicitud de ajuste administrativo
        private async void OnAjusteAdministrativoClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Ajuste Administrativo",
                "Tu solicitud de ajuste ha sido enviada al administrador. Recibirás una notificación cuando sea procesada.",
                "Aceptar");
        }

        // Navegación inferior: Inicio
        private async void OnInicioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeachersPage());
        }

        // Navegación inferior: Horario
        private async void OnHorarioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeachersHorarioPage());
        }

        // Navegación inferior: Perfil (ya estamos aquí)
        private void OnPerfilClicked(object sender, EventArgs e)
        {
            // Ya estamos en la página de perfil
        }
    }
}
