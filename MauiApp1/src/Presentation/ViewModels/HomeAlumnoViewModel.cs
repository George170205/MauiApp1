using System.Collections.ObjectModel;
using MauiApp1.src.Core.Models;
using Microsoft.Maui.Graphics;

namespace MauiApp1.src.Presentation.ViewModels
{
    public class HomeAlumnoViewModel
    {
        public string Nombre { get; set; } = "Josue Robledo";
        public string Iniciales => "JR";
        public string Carrera { get; set; } = "Ing. en Software";
        public int Asistencias { get; set; } = 94;
        public int Materias { get; set; } = 6;
        public double Promedio { get; set; } = 8.7;

        // Antes eran 3 strings sueltos, ahora un objeto ClaseInfo
        public ClaseInfo ProximaClase { get; set; }

        public ObservableCollection<Materia> MateriasList { get; set; }

        public HomeAlumnoViewModel()
        {
            ProximaClase = new ClaseInfo
            {
                Nombre = "Programación Avanzada",
                Salon = "Edificio A - Salón 204",
                HoraInicio = "12:30 PM",
                Acento = Color.FromArgb("#2563EB")
            };

            MateriasList = new ObservableCollection<Materia>
            {
                new Materia { Nombre = "Bases de Datos", Profesor = "Dra. María López",
                    Horario = "Lun y Mié - 8:00 AM", Salon = "A-205",
                    Acento = Color.FromArgb("#2563EB"), Porcentaje = 92 },
                new Materia { Nombre = "Redes de Computadoras", Profesor = "Ing. Carlos Sánchez",
                    Horario = "Mar y Jue - 2:00 PM", Salon = "C-102",
                    Acento = Color.FromArgb("#16A34A"), Porcentaje = 88 },
                new Materia { Nombre = "Inteligencia Artificial", Profesor = "Dr. Roberto Mendoza",
                    Horario = "Vie - 10:00 AM", Salon = "B-301",
                    Acento = Color.FromArgb("#F59E0B"), Porcentaje = 95 }
            };
        }
    }
}