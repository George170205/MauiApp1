using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Graphics;

namespace MauiApp1.src.Presentation.ViewModels.Student
{
    public class OpcionPerfil
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Icono { get; set; } 
        public Color ColorIcono { get; set; }
        public ICommand Comando { get; set; }
    }

    public class PerfilAlumnoViewModel : BindableObject
    {
        // Datos del Alumno
        public string NombreCompleto { get; set; } = "Josué Robledo";
        public string Iniciales { get; set; } = "JR";
        public string Matricula { get; set; } = "ALU-20210945";
        public string Carrera { get; set; } = "Ing. en Software";

        // Estadísticas Globales
        public int PorcentajeGlobal { get; set; } = 94;
        public int FaltasTotales { get; set; } = 3;

        // Lista de opciones
        public ObservableCollection<OpcionPerfil> OpcionesMenu { get; set; }

        public PerfilAlumnoViewModel()
        {
            OpcionesMenu = new ObservableCollection<OpcionPerfil>
            {
                new OpcionPerfil
                {
                    Titulo = "Justificar Faltas",
                    Descripcion = "Sube recetas médicas o documentos",
                    Icono = "documento.svg",
                    ColorIcono = Color.FromArgb("#2563EB"),
                    Comando = new Command(() => System.Diagnostics.Debug.WriteLine("Ir a Justificaciones"))
                },
                new OpcionPerfil
                {
                    Titulo = "Historial Detallado",
                    Descripcion = "Revisa tus asistencias por materia",
                    Icono = "historial.svg",
                    ColorIcono = Color.FromArgb("#16A34A"),
                    Comando = new Command(() => System.Diagnostics.Debug.WriteLine("Ir a Historial"))
                },
                new OpcionPerfil
                {
                    Titulo = "Configuración",
                    Descripcion = "Notificaciones y preferencias",
                    Icono = "ajustes.svg",
                    ColorIcono = Color.FromArgb("#6B7280"),
                    Comando = new Command(() => System.Diagnostics.Debug.WriteLine("Ir a Configuración"))
                }
            };
        }

        public ICommand CerrarSesionCommand => new Command(() =>
        {
            System.Diagnostics.Debug.WriteLine("Cerrando sesión...");
        });
    }
}