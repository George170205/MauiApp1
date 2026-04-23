using System.Windows.Input;
using MauiApp1.Services;
using MauiApp1.src.Core.Models;
using Microsoft.Maui.Graphics;

namespace MauiApp1.src.Presentation.ViewModels.Student
{
    public class AsistenciaViewModel : BindableObject
    {
        private readonly ApiService _apiService = new();

        // Inyectado desde la Page para abrir la cámara sin romper MVVM
        private readonly Func<Task<string?>>? _abrirEscaner;

        // ── Propiedades observables ──────────────────────────────────────────────

        public string FechaHoy { get; private set; }
        public string HoraActual { get; private set; }

        private string _estadoAsistencia = "Pendiente de registrar";
        public string EstadoAsistencia
        {
            get => _estadoAsistencia;
            private set { _estadoAsistencia = value; OnPropertyChanged(); }
        }

        private Color _colorEstado = Color.FromArgb("#6B7280");
        public Color ColorEstado
        {
            get => _colorEstado;
            private set { _colorEstado = value; OnPropertyChanged(); }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            private set { _isLoading = value; OnPropertyChanged(); }
        }

        public ClaseInfo ClaseActual { get; private set; }
        public ClaseInfo ProximaClase { get; private set; }

        // ── Comandos ─────────────────────────────────────────────────────────────

        public ICommand EscanearCommand { get; }
        public ICommand ManualCommand { get; }

        // ── Constructores ────────────────────────────────────────────────────────

        /// <summary>Constructor para unit tests — no realiza llamadas de red.</summary>
        public AsistenciaViewModel() : this(null) { }

        /// <summary>Constructor para uso real: recibe el delegate que abre la cámara.</summary>
        public AsistenciaViewModel(Func<Task<string?>>? abrirEscaner)
        {
            _abrirEscaner = abrirEscaner;

            FechaHoy = DateTime.Now.ToString("dddd d 'de' MMMM",
                new System.Globalization.CultureInfo("es-MX"));
            FechaHoy = char.ToUpper(FechaHoy[0]) + FechaHoy.Substring(1);

            HoraActual = DateTime.Now.ToString("hh:mm tt",
                new System.Globalization.CultureInfo("es-MX")).ToUpper();

            // Valores iniciales (tests los verifican; la API los sobreescribirá)
            EstadoAsistencia = "Asistencia pendiente — escanea el QR";

            ClaseActual = new ClaseInfo
            {
                Nombre = "Cargando...",
                Profesor = "—",
                HoraInicio = "--:--",
                HoraFin = "--:--",
                Salon = "—",
                Tipo = "—",
                Porcentaje = 0,
                Acento = Color.FromArgb("#2563EB")
            };

            ProximaClase = new ClaseInfo
            {
                Nombre = "Cargando...",
                Profesor = "—",
                HoraInicio = "--:--",
                Salon = "—",
                Acento = Color.FromArgb("#16A34A")
            };

            EscanearCommand = new Command(async () => await EscanearAsync(),
                () => !IsLoading);
            ManualCommand = new Command(async () => await IngresarManualAsync(),
                () => !IsLoading);
        }

        // ── Lógica de negocio ────────────────────────────────────────────────────

        private async Task EscanearAsync()
        {
            if (_abrirEscaner == null) return;

            string? qrToken;
            try
            {
                qrToken = await _abrirEscaner();
            }
            catch
            {
                MostrarEstado("No se pudo acceder a la cámara", "#EF4444");
                return;
            }

            if (string.IsNullOrEmpty(qrToken)) return;

            await EnviarAsistenciaAsync(qrToken);
        }

        private async Task IngresarManualAsync()
        {
            // La Page mostrará un prompt y llamará este método con el código
            // Por ahora la interacción manual requiere DisplayPromptAsync desde la Page
        }

        public async Task EnviarAsistenciaAsync(string qrToken)
        {
            IsLoading = true;
            ((Command)EscanearCommand).ChangeCanExecute();
            ((Command)ManualCommand).ChangeCanExecute();

            try
            {
                var idStr = Preferences.Get("estudiante_id", "0");
                if (!int.TryParse(idStr, out var estudianteId) || estudianteId == 0)
                {
                    MostrarEstado("Sesión no válida. Vuelve a iniciar sesión.", "#EF4444");
                    return;
                }

                var resultado = await _apiService.ValidarQR(qrToken, estudianteId);

                if (resultado.Exitoso)
                {
                    MostrarEstado("✓ Asistencia registrada", "#16A34A");
                }
                else
                {
                    var msg = resultado.MensajeEfectivo.ToLowerInvariant();
                    if (msg.Contains("expir"))
                        MostrarEstado("El código QR ha expirado", "#F59E0B");
                    else if (msg.Contains("ya") || msg.Contains("registrad"))
                        MostrarEstado("Ya registraste asistencia en esta clase", "#F59E0B");
                    else
                        MostrarEstado(resultado.MensajeEfectivo, "#EF4444");
                }
            }
            catch (UnauthorizedAccessException)
            {
                SesionExpirada?.Invoke(this, EventArgs.Empty);
            }
            catch (HttpRequestException)
            {
                MostrarEstado("Sin conexión. Intenta de nuevo.", "#EF4444");
            }
            catch
            {
                MostrarEstado("Error inesperado. Intenta de nuevo.", "#EF4444");
            }
            finally
            {
                IsLoading = false;
                ((Command)EscanearCommand).ChangeCanExecute();
                ((Command)ManualCommand).ChangeCanExecute();
            }
        }

        private void MostrarEstado(string mensaje, string hexColor)
        {
            EstadoAsistencia = mensaje;
            ColorEstado = Color.FromArgb(hexColor);
        }

        /// <summary>La Page se suscribe a este evento para navegar a //Login cuando el token expira.</summary>
        public event EventHandler? SesionExpirada;
    }
}
