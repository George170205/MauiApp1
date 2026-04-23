using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiApp1.Services;
using MauiApp1.src.Core.Models;
using Microsoft.Maui.Graphics;

namespace MauiApp1.src.Presentation.ViewModels.Student
{
    public class DiaFiltro : BindableObject
    {
        public string Letra { get; set; } = "";
        public string NombreCompleto { get; set; } = "";

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BgColor));
                OnPropertyChanged(nameof(TextColor));
            }
        }

        public Color BgColor => IsSelected ? Color.FromArgb("#2563EB") : Colors.Transparent;
        public Color TextColor => IsSelected ? Colors.White : Color.FromArgb("#6B7280");
    }

    public class HorarioAlumnoViewModel : BindableObject
    {
        private readonly ApiService _apiService = new();

        // Datos reales agrupados por día una vez que llega la API
        private Dictionary<string, List<ClaseInfo>> _horarioPorDia = new();
        private bool _datosApiCargados = false;

        // ── Propiedades observables ──────────────────────────────────────────────

        public ObservableCollection<DiaFiltro> DiasSemana { get; set; }
        public ObservableCollection<ClaseInfo> ClasesDelDia { get; set; }

        private string _diaActualTexto = "";
        public string DiaActualTexto
        {
            get => _diaActualTexto;
            set { _diaActualTexto = value; OnPropertyChanged(); }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private string _mensajeError = "";
        public string MensajeError
        {
            get => _mensajeError;
            set { _mensajeError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HayError)); }
        }
        public bool HayError => !string.IsNullOrEmpty(MensajeError);

        public ICommand SeleccionarDiaCommand { get; }

        // ── Evento para 401 ──────────────────────────────────────────────────────

        public event EventHandler? SesionExpirada;

        // ── Constructor ──────────────────────────────────────────────────────────

        public HorarioAlumnoViewModel()
        {
            ClasesDelDia = new ObservableCollection<ClaseInfo>();

            DiasSemana = new ObservableCollection<DiaFiltro>
            {
                new DiaFiltro { Letra = "L", NombreCompleto = "Lunes",     IsSelected = false },
                new DiaFiltro { Letra = "M", NombreCompleto = "Martes",    IsSelected = false },
                new DiaFiltro { Letra = "M", NombreCompleto = "Miércoles", IsSelected = true  },
                new DiaFiltro { Letra = "J", NombreCompleto = "Jueves",    IsSelected = false },
                new DiaFiltro { Letra = "V", NombreCompleto = "Viernes",   IsSelected = false }
            };

            SeleccionarDiaCommand = new Command<DiaFiltro>(SeleccionarDia);

            // Selección inicial → usa datos simulados (tests síncronos ven esto)
            SeleccionarDia(DiasSemana[2]);

            // Carga real en background — asíncrona, no bloquea los tests
            _ = CargarDesdeApiAsync();
        }

        // ── Selección de día ─────────────────────────────────────────────────────

        private void SeleccionarDia(DiaFiltro? diaSeleccionado)
        {
            if (diaSeleccionado == null) return;

            foreach (var dia in DiasSemana)
                dia.IsSelected = (dia == diaSeleccionado);

            DiaActualTexto = diaSeleccionado.NombreCompleto;

            if (_datosApiCargados)
                CargarClasesDesdeApi(diaSeleccionado.NombreCompleto);
            else
                CargarClasesSimuladas(diaSeleccionado.NombreCompleto);
        }

        // ── Datos simulados (fallback para tests y estado offline) ───────────────

        private void CargarClasesSimuladas(string dia)
        {
            ClasesDelDia.Clear();

            if (dia == "Miércoles")
            {
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Bases de Datos",         Profesor = "Dra. María López Ramírez",    HoraInicio = "8:00",  HoraFin = "10:00", Salon = "Edificio A - 205", Acento = Color.FromArgb("#2563EB") });
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Bases de Datos",         Profesor = "Dra. María López Ramírez",    HoraInicio = "10:00", HoraFin = "11:00", Salon = "Edificio A - 205", Acento = Color.FromArgb("#2563EB") });
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Redes de Computadoras",  Profesor = "Ing. Carlos Sánchez Torres",  HoraInicio = "11:00", HoraFin = "13:00", Salon = "Edificio C - 102", Acento = Color.FromArgb("#16A34A") });
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Inteligencia Artificial", Profesor = "Dr. Roberto Méndez Pérez",   HoraInicio = "13:00", HoraFin = "14:00", Salon = "Edificio B - 301", Acento = Color.FromArgb("#F59E0B") });
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Redes de Computadoras",  Profesor = "Ing. Carlos Sánchez Torres",  HoraInicio = "14:00", HoraFin = "15:00", Salon = "Edificio C - 102", Acento = Color.FromArgb("#16A34A") });
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Inteligencia Artificial", Profesor = "Dr. Roberto Méndez Pérez",   HoraInicio = "15:00", HoraFin = "16:00", Salon = "Edificio B - 301", Acento = Color.FromArgb("#F59E0B") });
            }
            else
            {
                ClasesDelDia.Add(new ClaseInfo
                {
                    Nombre = "Día Libre o Diferente",
                    Profesor = "Disfruta tu día",
                    HoraInicio = "00:00", HoraFin = "23:59",
                    Salon = "Casa",
                    Acento = Color.FromArgb("#6B7280")
                });
            }
        }

        // ── Datos reales desde la API ────────────────────────────────────────────

        public async Task CargarDesdeApiAsync()
        {
            var idStr = Preferences.Get("estudiante_id", "0");
            if (!int.TryParse(idStr, out var estudianteId) || estudianteId == 0)
                return;

            IsLoading = true;
            MensajeError = "";

            try
            {
                var horarios = await _apiService.GetHorario(estudianteId);

                if (horarios.Count == 0)
                    return;

                // Agrupar por día de semana
                _horarioPorDia.Clear();
                foreach (var h in horarios)
                {
                    var clase = new ClaseInfo
                    {
                        Nombre = h.Materia,
                        Profesor = h.Docente,
                        HoraInicio = h.HoraInicio,
                        HoraFin = h.HoraFin,
                        Salon = h.Aula,
                        Acento = ColorPorMateria(h.Materia)
                    };

                    if (!_horarioPorDia.TryGetValue(h.DiaSemana, out var lista))
                    {
                        lista = new List<ClaseInfo>();
                        _horarioPorDia[h.DiaSemana] = lista;
                    }
                    lista.Add(clase);
                }

                _datosApiCargados = true;

                // Refrescar la vista del día actualmente seleccionado
                var diaActivo = DiasSemana.FirstOrDefault(d => d.IsSelected);
                if (diaActivo != null)
                    CargarClasesDesdeApi(diaActivo.NombreCompleto);
            }
            catch (UnauthorizedAccessException)
            {
                SesionExpirada?.Invoke(this, EventArgs.Empty);
            }
            catch (HttpRequestException)
            {
                MensajeError = "Sin conexión. Mostrando horario local.";
            }
            catch
            {
                MensajeError = "Error al cargar el horario.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CargarClasesDesdeApi(string dia)
        {
            ClasesDelDia.Clear();

            if (_horarioPorDia.TryGetValue(dia, out var clases) && clases.Count > 0)
            {
                foreach (var c in clases)
                    ClasesDelDia.Add(c);
            }
            // Si no hay clases ese día: lista vacía (no se muestra "Día Libre")
        }

        private static Color ColorPorMateria(string materia)
        {
            // Asigna colores consistentes basándose en el hash del nombre
            var colores = new[] { "#2563EB", "#16A34A", "#F59E0B", "#7C3AED", "#DC2626", "#0891B2" };
            var idx = Math.Abs(materia.GetHashCode()) % colores.Length;
            return Color.FromArgb(colores[idx]);
        }
    }
}
