using System.Windows.Input;

namespace MauiApp1.src.Presentation.ViewModels.Teachers
{
    public enum TipoReporte { Diario, Semanal, Final }

    public class TeachersPerfilViewModel : BindableObject
    {
        // ── Datos del docente ─────────────────────────────
        public string Nombre { get; set; } = "Dr. Carlos Sánchez Torres";
        public string Departamento { get; set; } = "Ingeniería en Software";
        public string Correo { get; set; } = "carlos.sanchez@universidad.edu.mx";

        // ── Estado de reportes ────────────────────────────
        private TipoReporte? _ultimoReporteGenerado;
        public TipoReporte? UltimoReporteGenerado
        {
            get => _ultimoReporteGenerado;
            set
            {
                _ultimoReporteGenerado = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ReporteGenerado));
                OnPropertyChanged(nameof(MensajeReporte));
            }
        }

        public bool ReporteGenerado => _ultimoReporteGenerado.HasValue;

        public string MensajeReporte => _ultimoReporteGenerado switch
        {
            TipoReporte.Diario   => "Reporte diario generado correctamente.",
            TipoReporte.Semanal  => "Reporte semanal generado correctamente.",
            TipoReporte.Final    => "Reporte final generado correctamente.",
            _                    => string.Empty
        };

        // ── Configuración QR ──────────────────────────────
        private int _tiempoExpiracionQR = 5; // minutos por defecto
        public int TiempoExpiracionQR
        {
            get => _tiempoExpiracionQR;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "El tiempo debe ser mayor a 0.");
                _tiempoExpiracionQR = value;
                OnPropertyChanged();
            }
        }

        // ── Solicitud administrativa ──────────────────────
        private bool _ajusteEnviado;
        public bool AjusteEnviado
        {
            get => _ajusteEnviado;
            set
            {
                _ajusteEnviado = value;
                OnPropertyChanged();
            }
        }

        // ── Comandos ──────────────────────────────────────
        public ICommand GenerarReporteDiarioCommand { get; }
        public ICommand GenerarReporteSemanalCommand { get; }
        public ICommand GenerarReporteFinalCommand { get; }
        public ICommand EnviarAjusteAdministrativoCommand { get; }

        public TeachersPerfilViewModel()
        {
            GenerarReporteDiarioCommand = new Command(() =>
                UltimoReporteGenerado = TipoReporte.Diario);

            GenerarReporteSemanalCommand = new Command(() =>
                UltimoReporteGenerado = TipoReporte.Semanal);

            GenerarReporteFinalCommand = new Command(() =>
                UltimoReporteGenerado = TipoReporte.Final);

            EnviarAjusteAdministrativoCommand = new Command(() =>
                AjusteEnviado = true);
        }
    }
}
