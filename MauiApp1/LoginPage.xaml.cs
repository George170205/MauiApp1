using Microsoft.Maui.Controls;
using System;

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

        // Selección de rol Alumno
        private void OnAlumnoSelected(object sender, EventArgs e)
        {
            rolSeleccionado = "Alumno";
            ActualizarSeleccionRol();
        }

        // Selección de rol Docente
        private void OnDocenteSelected(object sender, EventArgs e)
        {
            rolSeleccionado = "Docente";
            ActualizarSeleccionRol();
        }

        // Selección de rol Admin
        private void OnAdminSelected(object sender, EventArgs e)
        {
            rolSeleccionado = "Admin";
            ActualizarSeleccionRol();
        }

        // Actualizar estilos visuales según rol seleccionado
        private void ActualizarSeleccionRol()
        {
            try
            {
                // Resetear todos los botones
                alumnoButton.Stroke = Color.FromArgb("#E5E7EB");
                alumnoButton.BackgroundColor = Colors.White;
                alumnoButton.StrokeThickness = 1;

                docenteButton.Stroke = Color.FromArgb("#E5E7EB");
                docenteButton.BackgroundColor = Colors.White;
                docenteButton.StrokeThickness = 1;

                adminButton.Stroke = Color.FromArgb("#E5E7EB");
                adminButton.BackgroundColor = Colors.White;
                adminButton.StrokeThickness = 1;

                // Marcar el seleccionado
                switch (rolSeleccionado)
                {
                    case "Alumno":
                        alumnoButton.Stroke = Color.FromArgb("#10B981");
                        alumnoButton.BackgroundColor = Color.FromArgb("#ECFDF5");
                        alumnoButton.StrokeThickness = 2;
                        break;
                    case "Docente":
                        docenteButton.Stroke = Color.FromArgb("#047857");
                        docenteButton.BackgroundColor = Color.FromArgb("#D1FAE5");
                        docenteButton.StrokeThickness = 2;
                        break;
                    case "Admin":
                        adminButton.Stroke = Color.FromArgb("#1F2937");
                        adminButton.BackgroundColor = Color.FromArgb("#F3F4F6");
                        adminButton.StrokeThickness = 2;
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ActualizarSeleccionRol: {ex.Message}");
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

                // Simular validación (aquí llamarías a tu API)
                await Task.Delay(1500);

                // TODO: Aquí validarías contra tu API/Base de datos
                // var resultado = await _authService.LoginAsync(usuarioEntry.Text, passwordEntry.Text, rolSeleccionado);

                // Por ahora, simular login exitoso
                bool loginExitoso = true; // Cambiar por validación real

                if (loginExitoso)
                {
                    // Guardar sesión si está marcado
                    if (recordarCheckBox.IsChecked)
                    {
                        // TODO: Guardar en Preferences
                        Preferences.Set("recordar_sesion", true);
                        Preferences.Set("usuario", usuarioEntry.Text);
                        Preferences.Set("rol", rolSeleccionado);
                    }

                    // Navegar según el rol
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
                        // await Navigation.PushAsync(new AlumnoDashboardPage());
                        await DisplayAlert("Bienvenido", "Navegando al Dashboard de Alumno...", "OK");
                        break;
                    case "Docente":
                        // await Navigation.PushAsync(new DocenteDashboardPage());
                        await DisplayAlert("Bienvenido", "Navegando al Dashboard de Docente...", "OK");
                        break;
                    case "Admin":
                        await Navigation.PushAsync(new MainPage());
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
                // TODO: Implementar cambio de idioma
            }
        }

        // Verificar si hay sesión guardada al cargar
        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                // Verificar si hay sesión guardada
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
                        ActualizarSeleccionRol();
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