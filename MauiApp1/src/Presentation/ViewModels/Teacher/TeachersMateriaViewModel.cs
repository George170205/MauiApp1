using System.Windows.Input;
using MauiApp1.src.Core.Models;
using Microsoft.Maui.Graphics;

namespace MauiApp1.src.Presentation.ViewModels.Teachers
{
    public class TeachersMateriaViewModel : BindableObject
    {
        // ── Datos de la materia ───────────────────────────
        public string NombreMateria { get; set; } = "Desarrollo Organizacional";
        public string Salon { get; set; } = "Edificio A - 205";
        public string Horario { get; set; } = "Lun y Mié - 8:00 AM";
        public int TotalAlumnos { get; set; } = 32;
        public int AlumnosPresentes { get; set; } = 28;

        public double PorcentajeAsistencia =>
            TotalAlumnos > 0 ? Math.Round((double)AlumnosPresentes / TotalAlumnos * 100, 1) : 0;

        // ── Estado del reporte ────────────────────────────
        private bool _reporteGenerado;
        public bool ReporteGenerado
        {
            get => _reporteGenerado;
            set
            {
                _reporteGenerado = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MensajeReporte));
            }
        }

        public string MensajeReporte => ReporteGenerado
            ? "El reporte se ha generado exitosamente."
            : string.Empty;

        // ── Estado del QR ─────────────────────────────────
        private bool _qrRegenerado;
        public bool QrRegenerado
        {
            get => _qrRegenerado;
            set
            {
                _qrRegenerado = value;
                OnPropertyChanged();
            }
        }

        // ── Comandos ──────────────────────────────────────
        public ICommand GenerarReporteCommand { get; }
        public ICommand RegenerarQRCommand { get; }

        public TeachersMateriaViewModel()
        {
            GenerarReporteCommand = new Command(() =>
            {
                ReporteGenerado = true;
            });

            RegenerarQRCommand = new Command(() =>
            {
                QrRegenerado = true;
            });
        }
    }
}
