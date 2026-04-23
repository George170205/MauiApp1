using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiApp1.Services;
using Microsoft.Maui.Graphics;

namespace MauiApp1.src.Presentation.ViewModels.Student
{
    public class OpcionPerfil
    {
        public string Titulo { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public string Icono { get; set; } = "";
        public Color ColorIcono { get; set; }
        public ICommand? Comando { get; set; }
    }

    public class PerfilAlumnoViewModel : BindableObject
    {
        private readonly ApiService _apiService = new();
        private readonly Func<Task>? _onLogout;

        // ── Propiedades observables ──────────────────────────────────────────────

        private string _nombreCompleto = "Josué Robledo";
        public string NombreCompleto
        {
            get => _nombreCompleto;
            set { _nombreCompleto = value; OnPropertyChanged(); OnPropertyChanged(nameof(Iniciales)); }
        }

        public string Iniciales
        {
            get
            {
                var partes = NombreCompleto.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return partes.Length >= 2
                    ? $"{partes[0][0]}{partes[1][0]}".ToUpper()
                    : partes.Length == 1 ? partes[0][0].ToString().ToUpper() : "?";
            }
        }

        private string _matricula = "ALU-20210945";
        public string Matricula
        {
            get => _matricula;
            set { _matricula = value; OnPropertyChanged(); }
        }

        private string _carrera = "Ing. en Software";
        public string Carrera
        {
            get => _carrera;
            set { _carrera = value; OnPropertyChanged(); }
        }

        private int _porcentajeGlobal = 94;
        public int PorcentajeGlobal
        {
            get => _porcentajeGlobal;
            set { _porcentajeGlobal = value; OnPropertyChanged(); }
        }

        private int _faltasTotales = 3;
        public int FaltasTotales
        {
            get => _faltasTotales;
            set { _faltasTotales = value; OnPropertyChanged(); }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public ObservableCollection<OpcionPerfil> OpcionesMenu { get; }

        // ── Comandos ─────────────────────────────────────────────────────────────

        public ICommand CerrarSesionCommand { get; }

        // ── Evento para 401 ──────────────────────────────────────────────────────

        public event EventHandler? SesionExpirada;

        // ── Constructor ──────────────────────────────────────────────────────────

        public PerfilAlumnoViewModel() : this(null) { }

        public PerfilAlumnoViewModel(Func<Task>? onLogout)
        {
            _onLogout = onLogout;

            OpcionesMenu = new ObservableCollection<OpcionPerfil>
            {
                new OpcionPerfil
                {
                    Titulo = "Justificar Faltas",
                    Descripcion = "Sube recetas médicas o documentos",
                    Icono = "documento.svg",
                    ColorIcono = Color.FromArgb("#2563EB"),
                    Comando = new Command(() => { /* próximamente */ })
                },
                new OpcionPerfil
                {
                    Titulo = "Historial Detallado",
                    Descripcion = "Revisa tus asistencias por materia",
                    Icono = "historial.svg",
                    ColorIcono = Color.FromArgb("#16A34A"),
                    Comando = new Command(() => { /* próximamente */ })
                },
                new OpcionPerfil
                {
                    Titulo = "Configuración",
                    Descripcion = "Notificaciones y preferencias",
                    Icono = "ajustes.svg",
                    ColorIcono = Color.FromArgb("#6B7280"),
                    Comando = new Command(() => { /* próximamente */ })
                }
            };

            CerrarSesionCommand = new Command(async () => await LogoutAsync());

            _ = CargarDesdeApiAsync();
        }

        // ── Carga desde API ──────────────────────────────────────────────────────

        public async Task CargarDesdeApiAsync()
        {
            var idStr = Preferences.Get("estudiante_id", "0");
            if (!int.TryParse(idStr, out var estudianteId) || estudianteId == 0)
                return;

            IsLoading = true;

            try
            {
                var dto = await _apiService.GetHomeAlumno(estudianteId);
                if (dto == null) return;

                NombreCompleto = dto.Nombre;
                Matricula = dto.Matricula;
                Carrera = dto.Carrera;
                PorcentajeGlobal = (int)Math.Round(dto.PorcentajeAsistencia);
            }
            catch (UnauthorizedAccessException)
            {
                SesionExpirada?.Invoke(this, EventArgs.Empty);
            }
            catch { /* Sin conexión — muestra datos en caché */ }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Logout ───────────────────────────────────────────────────────────────

        private async Task LogoutAsync()
        {
            try
            {
                await _apiService.Logout();
            }
            finally
            {
                Preferences.Clear();

                if (_onLogout != null)
                    await _onLogout();
            }
        }
    }
}
