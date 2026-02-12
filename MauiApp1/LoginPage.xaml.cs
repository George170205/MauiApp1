using Microsoft.Maui.Controls;
using System;
using MauiApp1.Data;

namespace MauiApp1
{
    public partial class LoginPage : ContentPage
    {
        private string rolSeleccionado = "Alumno"; // Por defecto Alumno

        public LoginPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en LoginPage InitializeComponent: {ex.Message}");
            }
        }

        // Método para el Picker
        private void OnRolSelected(object sender, EventArgs e)
        {
            try
            {
                var picker = (Picker)sender;
                int selectedIndex = picker.SelectedIndex;

                switch (selectedIndex)
                {
                    case 0:
                        rolSeleccionado = "Alumno";
                        break;
                    case 1:
                        rolSeleccionado = "Docente";
                        break;
                    case 2:
                        rolSeleccionado = "Admin";
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OnRolSelected: {ex.Message}");
            }
        }

        // Iniciar Sesión
        private async void OnIniciarSesionClicked(object sender, EventArgs e)
        {
            try
            {
                // Validar campos
                if (string.IsNullOrWhiteSpace(usuarioEntry.Text))
                {
                    await DisplayAlert("Error", "Por favor ingresa tu usuario o matrícula", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(passwordEntry.Text))
                {
                    await DisplayAlert("Error", "Por favor ingresa tu contraseña", "OK");
                    return;
                }

                // Deshabilitar botón mientras procesa
                var button = (Button)sender;
                button.IsEnabled = false;
                button.Text = "Iniciando...";

                // Simular validación
                await Task.Delay(1500);

                bool loginExitoso = true;

                if (loginExitoso)
                {
                    // Guardar sesión si está marcado
                    if (recordarCheckBox.IsChecked)
                    {
                        Preferences.Set("recordar_sesion", true);
                        Preferences.Set("usuario", usuarioEntry.Text);
                        Preferences.Set("rol", rolSeleccionado);
                    }

                    await NavegarSegunRol();
                }
                else
                {
                    await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
                    button.IsEnabled = true;
                    button.Text = "Iniciar Sesión";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al iniciar sesión: {ex.Message}", "OK");
                var button = (Button)sender;
                button.IsEnabled = true;
                button.Text = "Iniciar Sesión";
            }
        }

        // Navegar según rol
        private async Task NavegarSegunRol()
        {
            try
            {
                switch (rolSeleccionado)
                {
                    case "Alumno":
                        await DisplayAlert("Bienvenido", "Navegando al Dashboard de Alumno...", "OK");
                        break;
                    case "Docente":
                        await DisplayAlert("Bienvenido", "Navegando al Dashboard de Docente...", "OK");
                        break;
                    case "Admin":
                        Application.Current.MainPage = new MainPage();
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al navegar: {ex.Message}", "OK");
            }
        }

        // Ayuda
        private async void OnAyudaClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Ayuda",
                "¿Olvidaste tu contraseña?\n\n" +
                "Contacta al administrador del sistema:\n" +
                "📧 admin@escuela.edu\n" +
                "📞 664-123-4567",
                "Entendido");
        }

        // Idioma
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

        // Verificar sesión guardada
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                bool ok = TestConnection.CanConnect();
                await DisplayAlert("SQL", ok ? "Conectado" : "Error de conexión", "OK");

                bool recordarSesion = Preferences.Get("recordar_sesion", false);

                if (recordarSesion)
                {
                    string usuarioGuardado = Preferences.Get("usuario", "");
                    string rolGuardado = Preferences.Get("rol", "Alumno");

                    if (!string.IsNullOrEmpty(usuarioGuardado))
                    {
                        usuarioEntry.Text = usuarioGuardado;
                        rolSeleccionado = rolGuardado;
                        recordarCheckBox.IsChecked = true;

                        // Seleccionar el rol guardado en el Picker
                        switch (rolGuardado)
                        {
                            case "Alumno":
                                rolPicker.SelectedIndex = 0;
                                break;
                            case "Docente":
                                rolPicker.SelectedIndex = 1;
                                break;
                            case "Admin":
                                rolPicker.SelectedIndex = 2;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OnAppearing: {ex.Message}");
            }
        }
    }
}