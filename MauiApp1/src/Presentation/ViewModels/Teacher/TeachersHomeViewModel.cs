using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiApp1.src.Core.Models;
using Microsoft.Maui.Graphics;

namespace MauiApp1.src.Presentation.ViewModels.Teachers
{
    public class TeachersHomeViewModel : BindableObject
    {
        // ── Datos del docente ─────────────────────────────
        public string Nombre { get; set; } = "Dr. Carlos Sánchez Torres";
        public string Iniciales => "CS";
        public string Departamento { get; set; } = "Ing. en Software";
        public int TotalAlumnos { get; set; } = 87;
        public int TotalMaterias { get; set; } = 4;
        public double PromedioAsistencia { get; set; } = 91.5;

        // ── Próxima clase ─────────────────────────────────
        public ClaseInfo ProximaClase { get; set; }

        // ── Lista de materias ─────────────────────────────
        public ObservableCollection<Materia> MateriasList { get; set; }

        // ── Estado del QR ─────────────────────────────────
        private bool _qrActivo;
        public bool QrActivo
        {
            get => _qrActivo;
            set
            {
                _qrActivo = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EstadoQrTexto));
            }
        }

        public string EstadoQrTexto => QrActivo ? "QR Activo" : "QR Inactivo";

        // ── Sesión ────────────────────────────────────────
        private bool _sesionCerrada;
        public bool SesionCerrada
        {
            get => _sesionCerrada;
            set
            {
                _sesionCerrada = value;
                OnPropertyChanged();
            }
        }

        // ── Comandos ──────────────────────────────────────
        public ICommand RegenerarQRCommand { get; }
        public ICommand CerrarSesionCommand { get; }
        public ICommand VerClaseCommand { get; }

        public TeachersHomeViewModel()
        {
            ProximaClase = new ClaseInfo
            {
                Nombre = "Redes de Computadoras",
                Salon = "Edificio C - Salón 102",
                HoraInicio = "2:00 PM",
                Acento = Color.FromArgb("#2563EB")
            };

            MateriasList = new ObservableCollection<Materia>
            {
                new Materia { Nombre = "Redes de Computadoras", Profesor = "Ing. Carlos Sánchez",
                    Horario = "Mar y Jue - 2:00 PM", Salon = "C-102",
                    Acento = Color.FromArgb("#2563EB"), Porcentaje = 88 },
                new Materia { Nombre = "Bases de Datos", Profesor = "Ing. Carlos Sánchez",
                    Horario = "Lun y Mié - 8:00 AM", Salon = "A-205",
                    Acento = Color.FromArgb("#16A34A"), Porcentaje = 92 },
                new Materia { Nombre = "Programación Avanzada", Profesor = "Ing. Carlos Sánchez",
                    Horario = "Vie - 10:00 AM", Salon = "B-301",
                    Acento = Color.FromArgb("#F59E0B"), Porcentaje = 85 },
            };

            RegenerarQRCommand = new Command(() =>
            {
                QrActivo = true;
            });

            CerrarSesionCommand = new Command(() =>
            {
                SesionCerrada = true;
            });

            VerClaseCommand = new Command(() =>
            {
                System.Diagnostics.Debug.WriteLine("Navegando a detalle de clase");
            });
        }
    }
}
