using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public partial class TeachersHorarioPage : ContentPage
    {
        // Día actualmente seleccionado
        private string _diaSeleccionado = "Lunes";

        public TeachersHorarioPage()
        {
            InitializeComponent();
        }

        // Botón de regreso (←)
        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Selector de día (si agregas botones por día en el XAML, usa este patrón)
        private void OnDiaSeleccionado(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Text != null)
            {
                _diaSeleccionado = btn.Text;
                // Aquí podrías actualizar la UI para mostrar las clases del día seleccionado
            }
        }

        // Tap en una clase del horario para ver detalle
        private async void OnClaseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeachersMateriaPage());
        }

        // Navegación inferior: Inicio
        private async void OnInicioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeachersPage());
        }

        // Navegación inferior: Horario (ya estamos aquí)
        private void OnHorarioClicked(object sender, EventArgs e)
        {
            // Ya estamos en la página de horario
        }

        // Navegación inferior: Perfil
        private async void OnPerfilClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeachersPerfilPage());
        }
    }
}
