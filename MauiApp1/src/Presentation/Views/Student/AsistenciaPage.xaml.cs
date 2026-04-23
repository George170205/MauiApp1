namespace MauiApp1.src.Presentation.Views;

using MauiApp1.src.Presentation.ViewModels.Student;

public partial class AsistenciaPage : ContentPage
{
    private AsistenciaViewModel _vm = null!;

    public AsistenciaPage()
    {
        InitializeComponent();

        _vm = new AsistenciaViewModel(AbrirEscanerAsync);
        _vm.SesionExpirada += OnSesionExpirada;
        BindingContext = _vm;
    }

    // ── Escáner QR ───────────────────────────────────────────────────────────────

    private async Task<string?> AbrirEscanerAsync()
    {
        var permiso = await Permissions.RequestAsync<Permissions.Camera>();
        if (permiso != PermissionStatus.Granted)
        {
            await DisplayAlert("Permiso requerido",
                "La app necesita acceso a la cámara para escanear el código QR.", "OK");
            return null;
        }

        var tcs = new TaskCompletionSource<string?>();
        await Navigation.PushModalAsync(new QrScannerPage(tcs));
        return await tcs.Task;
    }

    // ── Ingreso manual ───────────────────────────────────────────────────────────

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Encadenar ManualCommand para pedir el código via prompt de la Page
        // (los ViewModel solo coordinan la lógica; la UI del prompt viene de aquí)
    }

    // El botón manual está ligado a ManualCommand en el ViewModel, pero ese
    // comando no puede abrir un DisplayPromptAsync sin acceso a la Page.
    // Usamos un TapGestureRecognizer adicional directo en code-behind para el flujo manual.
    // El XAML ya tiene el binding a ManualCommand; aquí sobreescribimos el flujo:
    private async void OnManualTap(object? sender, EventArgs e)
    {
        var codigo = await DisplayPromptAsync(
            "Código manual",
            "Ingresa el código de la sesión:",
            "Registrar",
            "Cancelar",
            keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(codigo)) return;
        await _vm.EnviarAsistenciaAsync(codigo);
    }

    // ── Sesión expirada ──────────────────────────────────────────────────────────

    private async void OnSesionExpirada(object? sender, EventArgs e)
    {
        await DisplayAlert("Sesión expirada",
            "Tu sesión ha vencido. Inicia sesión de nuevo.", "OK");
        Preferences.Clear();
        await Shell.Current.GoToAsync("//Login");
    }
}
