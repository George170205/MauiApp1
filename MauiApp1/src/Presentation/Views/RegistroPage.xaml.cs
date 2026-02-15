using System.Text.RegularExpressions;
using MauiApp1.Services;
using MauiApp1.src.Core.Models;


namespace MauiApp1;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class RegistroPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public RegistroPage()
    {
        InitializeComponent();
    }

    // ==========================================
    // VALIDACIONES EN TIEMPO REAL
    // ==========================================

    private void OnEmailTextChanged(object sender, TextChangedEventArgs e)
    {
        string email = emailEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(email))
        {
            emailBorder.Stroke = Color.FromArgb("#E5E7EB");
            emailErrorLabel.IsVisible = false;
            return;
        }

        if (!IsValidEmail(email))
        {
            emailBorder.Stroke = Color.FromArgb("#EF4444");
            emailErrorLabel.Text = "❌ Formato de email inválido";
            emailErrorLabel.IsVisible = true;
        }
        else
        {
            emailBorder.Stroke = Color.FromArgb("#10B981");
            emailErrorLabel.IsVisible = false;
        }
    }

    private void OnPasswordTextChanged(object sender, TextChangedEventArgs e)
    {
        string password = passwordEntry.Text ?? "";

        if (string.IsNullOrWhiteSpace(password))
        {
            passwordBorder.Stroke = Color.FromArgb("#E5E7EB");
            passwordErrorLabel.IsVisible = false;
            return;
        }

        if (password.Length < 6)
        {
            passwordBorder.Stroke = Color.FromArgb("#EF4444");
            passwordErrorLabel.Text = "❌ Mínimo 6 caracteres";
            passwordErrorLabel.IsVisible = true;
        }
        else
        {
            passwordBorder.Stroke = Color.FromArgb("#10B981");
            passwordErrorLabel.IsVisible = false;
        }

        OnConfirmarPasswordTextChanged(sender, null);
    }

    private void OnConfirmarPasswordTextChanged(object sender, TextChangedEventArgs e)
    {
        string password = passwordEntry.Text ?? "";
        string confirmar = confirmarPasswordEntry.Text ?? "";

        if (string.IsNullOrWhiteSpace(confirmar))
        {
            confirmarPasswordBorder.Stroke = Color.FromArgb("#E5E7EB");
            confirmarPasswordErrorLabel.IsVisible = false;
            return;
        }

        if (password != confirmar)
        {
            confirmarPasswordBorder.Stroke = Color.FromArgb("#EF4444");
            confirmarPasswordErrorLabel.Text = "❌ Las contraseñas no coinciden";
            confirmarPasswordErrorLabel.IsVisible = true;
        }
        else
        {
            confirmarPasswordBorder.Stroke = Color.FromArgb("#10B981");
            confirmarPasswordErrorLabel.IsVisible = false;
        }
    }

    // ==========================================
    // VALIDAR FORMULARIO
    // ==========================================

    private bool ValidarFormulario()
    {
        if (rolPicker.SelectedIndex == -1)
        {
            MostrarAlerta("Error", "Por favor selecciona tu rol");
            return false;
        }

        string email = emailEntry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
        {
            MostrarAlerta("Error", "Email inválido");
            return false;
        }

        if (string.IsNullOrWhiteSpace(nombreEntry.Text))
        {
            MostrarAlerta("Error", "El nombre es obligatorio");
            return false;
        }

        if (string.IsNullOrWhiteSpace(apellidoEntry.Text))
        {
            MostrarAlerta("Error", "El apellido es obligatorio");
            return false;
        }

        string password = passwordEntry.Text ?? "";
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            MostrarAlerta("Error", "La contraseña debe tener mínimo 6 caracteres");
            return false;
        }

        if (password != confirmarPasswordEntry.Text)
        {
            MostrarAlerta("Error", "Las contraseñas no coinciden");
            return false;
        }

        if (!terminosCheckBox.IsChecked)
        {
            MostrarAlerta("Error", "Debes aceptar los términos");
            return false;
        }

        return true;
    }

    // ==========================================
    // REGISTRAR USUARIO (LLAMANDO A LA API)
    // ==========================================

    private async void OnRegistrarClicked(object sender, EventArgs e)
    {
        if (!ValidarFormulario())
            return;

        registrarButton.IsEnabled = false;
        registrarButton.Text = "Registrando...";

        try
        {
            var request = new RegisterRequest
            {
                RolID = ObtenerRolID(),
                Email = emailEntry.Text.Trim(),
                Password = passwordEntry.Text,
                Nombre = nombreEntry.Text.Trim(),
                Apellido = apellidoEntry.Text.Trim(),
                Telefono = string.IsNullOrWhiteSpace(telefonoEntry.Text)
        ? null
        : telefonoEntry.Text.Trim()
            };

            bool success = await _apiService.RegisterUser(request);


            if (success)
            {
                await DisplayAlert("✅ Registro Exitoso",
                    "Tu cuenta ha sido creada correctamente.",
                    "OK");

                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error",
                    "No se pudo registrar el usuario.",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            registrarButton.IsEnabled = true;
            registrarButton.Text = "Crear Cuenta";
        }
    }

    // ==========================================
    // MÉTODOS AUXILIARES
    // ==========================================
    private int ObtenerRolID()
    {
        return rolPicker.SelectedIndex switch
        {
            0 => 1, // Alumno
            1 => 2, // Docente
            2 => 3, // Admin
            _ => 1
        };
    }

    private bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    private async void MostrarAlerta(string titulo, string mensaje)
    {
        await DisplayAlert(titulo, mensaje, "OK");
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnIniciarSesionClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnTerminosClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Términos y Condiciones",
            "Al crear una cuenta, aceptas nuestros términos.",
            "Entendido");
    }
}
