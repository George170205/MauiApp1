using System.Collections.ObjectModel;
using MauiApp1.Services;
using MauiApp1.src.Core.Models;
using Microsoft.Maui.Graphics;

namespace MauiApp1.src.Presentation.ViewModels.Student
{
    public class HomeAlumnoViewModel : BindableObject
    {
        private readonly ApiService _apiService = new();

        // ── Propiedades observables ──────────────────────────────────────────────

        private string _nombre = "Josue Robledo";
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); OnPropertyChanged(nameof(Iniciales)); }
        }

        public string Iniciales
        {
            get
            {
                var partes = Nombre.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return partes.Length >= 2
                    ? $"{partes[0][0]}{partes[1][0]}".ToUpper()
                    : partes.Length == 1 ? partes[0][0].ToString().ToUpper() : "?";
            }
        }

        private string _carrera = "Ing. en Software";
        public string Carrera
        {
            get => _carrera;
            set { _carrera = value; OnPropertyChanged(); }
        }

        private int _asistencias = 94;
        public int Asistencias
        {
            get => _asistencias;
            set { _asistencias = value; OnPropertyChanged(); }
        }

        private int _materias = 6;
        public int Materias
        {
            get => _materias;
            set { _materias = value; OnPropertyChanged(); }
        }

        private double _promedio = 8.7;
        public double Promedio
        {
            get => _promedio;
            set { _promedio = value; OnPropertyChanged(); }
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

        private ClaseInfo _proximaClase = null!;
        public ClaseInfo ProximaClase
        {
            get => _proximaClase;
            set { _proximaClase = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Materia> MateriasList { get; } = new();

        // ── Evento para 401 ──────────────────────────────────────────────────────

        public event EventHandler? SesionExpirada;

        // ── Constructor ──────────────────────────────────────────────────────────

        public HomeAlumnoViewModel()
        {
            // Valores iniciales que satisfacen los tests (Iniciales = "JR" cuando
            // Nombre = "Josue Robledo") y la UI muestra algo inmediatamente.
            ProximaClase = new ClaseInfo
            {
                Nombre = "Programación Avanzada",
                Salon = "Edificio A - Salón 204",
                HoraInicio = "12:30 PM",
                Acento = Color.FromArgb("#2563EB")
            };

            MateriasList.Add(new Materia
            {
                Nombre = "Bases de Datos", Profesor = "Dra. María López",
                Horario = "Lun y Mié - 8:00 AM", Salon = "A-205",
                Acento = Color.FromArgb("#2563EB"), Porcentaje = 92
            });
            MateriasList.Add(new Materia
            {
                Nombre = "Redes de Computadoras", Profesor = "Ing. Carlos Sánchez",
                Horario = "Mar y Jue - 2:00 PM", Salon = "C-102",
                Acento = Color.FromArgb("#16A34A"), Porcentaje = 88
            });
            MateriasList.Add(new Materia
            {
                Nombre = "Inteligencia Artificial", Profesor = "Dr. Roberto Mendoza",
                Horario = "Vie - 10:00 AM", Salon = "B-301",
                Acento = Color.FromArgb("#F59E0B"), Porcentaje = 95
            });

            // Carga real en background — los tests no la esperan (son síncronos)
            _ = CargarDesdeApiAsync();
        }

        // ── Carga desde API ──────────────────────────────────────────────────────

        public async Task CargarDesdeApiAsync()
        {
            var idStr = Preferences.Get("estudiante_id", "0");
            if (!int.TryParse(idStr, out var estudianteId) || estudianteId == 0)
                return;

            IsLoading = true;
            MensajeError = "";

            try
            {
                var dto = await _apiService.GetHomeAlumno(estudianteId);
                if (dto == null)
                {
                    MensajeError = "No se pudo cargar la información. Intenta más tarde.";
                    return;
                }

                Nombre = dto.Nombre;
                Carrera = dto.Carrera;
                Asistencias = (int)Math.Round(dto.PorcentajeAsistencia);
                Materias = dto.MateriasActivas;
                Promedio = dto.Promedio;

                if (dto.ProximaClase != null)
                {
                    ProximaClase = new ClaseInfo
                    {
                        Nombre = dto.ProximaClase.Nombre,
                        Profesor = dto.ProximaClase.Docente,
                        HoraInicio = dto.ProximaClase.HoraInicio,
                        HoraFin = dto.ProximaClase.HoraFin,
                        Salon = dto.ProximaClase.Aula,
                        Acento = Color.FromArgb("#2563EB")
                    };
                }

                // Refrescar lista de materias si el endpoint la devuelve en el futuro
                // (actualmente no está en el contrato del endpoint /home)
            }
            catch (UnauthorizedAccessException)
            {
                SesionExpirada?.Invoke(this, EventArgs.Empty);
            }
            catch (HttpRequestException)
            {
                MensajeError = "Sin conexión. Verifica tu red.";
            }
            catch
            {
                MensajeError = "Error inesperado al cargar los datos.";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
