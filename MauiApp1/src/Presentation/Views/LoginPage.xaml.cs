using Microsoft.Maui.Controls;
using System;
using MauiApp1.Data;
using MauiApp1.Services;
using MauiApp1.src.Core.Models;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;



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
            if (string.IsNullOrWhiteSpace(usuarioEntry.Text) ||
                string.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                await DisplayAlert("Error", "Completa los campos", "OK");
                return;
            }

            try
            {
                var request = new LoginRequest
                {
                    Email = usuarioEntry.Text.Trim(),
                    Password = passwordEntry.Text
                };

                var response = await _apiService.LoginUser(request);

                if (response == null || string.IsNullOrEmpty(response.Token))
                {
                    await DisplayAlert("Error", "Credenciales incorrectas", "OK");
                    return;
                }

                var token = response.Token;

                // 🔐 Guardar token
                Preferences.Set("jwt_token", token);

                // 🔥 Decodificar JWT
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                var rolID = jwt.Claims.FirstOrDefault(c => c.Type == "RolID")?.Value;
                var email = jwt.Claims.FirstOrDefault(c => c.Type.Contains("emailaddress"))?.Value;

                Preferences.Set("rolID", rolID ?? "");
                Preferences.Set("usuario", email ?? "");

                await DisplayAlert("Bienvenido", $"Usuario: {email}", "OK");

                await NavegarSegunRolID(rolID);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task NavegarSegunRolID(string rolID)
        {
            switch (rolID)
            {
                case "1":
                    await DisplayAlert("Rol", "Entrando como Alumno", "OK");
                    break;

                case "2":
                    await DisplayAlert("Rol", "Entrando como Docente", "OK");
                    break;

                case "3":
                    await Navigation.PushAsync(new MainPage());
                    break;

                default:
                    await DisplayAlert("Error", "Rol no reconocido", "OK");
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

        private readonly ApiService _apiService = new ApiService();

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