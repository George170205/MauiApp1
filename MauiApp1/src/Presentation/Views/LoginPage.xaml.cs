using Microsoft.Maui.Controls;
using System;
using MauiApp1.Data;

namespace MauiApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)] // ← Optimización
    public partial class LoginPage : ContentPage
    {
        private string rolSeleccionado = "Alumno";

        public LoginPage()
        {
            InitializeComponent();
        }

        private void OnRolSelected(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            switch (selectedIndex)
            {
                case 0: rolSeleccionado = "Alumno"; break;
                case 1: rolSeleccionado = "Docente"; break;
                case 2: rolSeleccionado = "Admin"; break;
            }
        }

        private async void OnRegistroClicked(object sender, EventArgs e)
        {
            // Navegación rápida sin animación
            await Navigation.PushAsync(new RegistroPage(), animated: true);
        }

        private async void OnIniciarSesionClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(usuarioEntry.Text))
            {
                await DisplayAlert("Error", "Ingresa tu usuario", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                await DisplayAlert("Error", "Ingresa tu contraseña", "OK");
                return;
            }

            // TODO: Validación real contra BD
            await NavegarSegunRol();
        }

        private async Task NavegarSegunRol()
        {
            switch (rolSeleccionado)
            {
                case "Alumno":
                    await DisplayAlert("Bienvenido", "Dashboard Alumno", "OK");
                    break;
                case "Docente":
                    await DisplayAlert("Bienvenido", "Dashboard Docente", "OK");
                    break;
                case "Admin":
                    await Navigation.PushAsync(new MainPage());
                    break;
            }
        }

        private async void OnAyudaClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Ayuda", "📧 admin@escuela.edu", "OK");
        }

        private async void OnIdiomaClicked(object sender, EventArgs e)
        {
            var idioma = await DisplayActionSheet("Idioma", "Cancelar", null, "🇲🇽 Español", "🇺🇸 English");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // ❌ ELIMINADA la validación de BD que causaba lentitud

            bool recordarSesion = Preferences.Get("recordar_sesion", false);

            if (recordarSesion)
            {
                usuarioEntry.Text = Preferences.Get("usuario", "");
                string rolGuardado = Preferences.Get("rol", "Alumno");
                rolSeleccionado = rolGuardado;
                recordarCheckBox.IsChecked = true;

                rolPicker.SelectedIndex = rolGuardado switch
                {
                    "Alumno" => 0,
                    "Docente" => 1,
                    "Admin" => 2,
                    _ => 0
                };
            }
        }
    }
}