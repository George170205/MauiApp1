using System.Windows.Input;
using Microsoft.Maui.Graphics;
using MauiApp1.src.Core.Models;

namespace MauiApp1.src.Presentation.ViewModels
{
  

    public class AsistenciaViewModel
    {
        public string FechaHoy { get; set; }
        public ClaseInfo ClaseActual { get; set; }
        public ClaseInfo ProximaClase { get; set; }

        public ICommand EscanearCommand { get; }
        public ICommand ManualCommand { get; }

        public AsistenciaViewModel()
        {
            FechaHoy = DateTime.Now.ToString("dddd d 'de' MMMM",
                new System.Globalization.CultureInfo("es-MX"));

            // Primera letra en mayúscula
            FechaHoy = char.ToUpper(FechaHoy[0]) + FechaHoy.Substring(1);

            ClaseActual = new ClaseInfo
            {
                Nombre = "Bases de Datos",
                Profesor = "Dr. Camilo Caraveo Mena",
                HoraInicio = "8:00 AM",
                HoraFin = "10:00 AM",
                Salon = "A-205",
                Tipo = "Taller",
                Porcentaje = 92,
                Acento = Color.FromArgb("#2563EB")
            };

            ProximaClase = new ClaseInfo
            {
                Nombre = "Lenguaje C",
                Profesor = "Dr. Chespirito Hernandez",
                Iniciales = "LC",
                Acento = Color.FromArgb("#16A34A")
            };

            EscanearCommand = new Command(() =>
            {
                System.Diagnostics.Debug.WriteLine("Escanear QR");
            });

            ManualCommand = new Command(() =>
            {
                System.Diagnostics.Debug.WriteLine("Ingresar código manual");
            });
        }
    }
}