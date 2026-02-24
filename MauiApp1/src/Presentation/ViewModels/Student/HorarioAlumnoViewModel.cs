using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiApp1.src.Core.Models;
using Microsoft.Maui.Graphics;

namespace MauiApp1.src.Presentation.ViewModels.Student
{
 
    public class DiaFiltro : BindableObject
    {
        public string Letra { get; set; }
        public string NombreCompleto { get; set; }

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
        public ObservableCollection<DiaFiltro> DiasSemana { get; set; }
        public ObservableCollection<ClaseInfo> ClasesDelDia { get; set; }

        private string _diaActualTexto;
        public string DiaActualTexto
        {
            get => _diaActualTexto;
            set
            {
                _diaActualTexto = value;
                OnPropertyChanged();
            }
        }

        public ICommand SeleccionarDiaCommand { get; }

        public HorarioAlumnoViewModel()
        {
            ClasesDelDia = new ObservableCollection<ClaseInfo>();

            DiasSemana = new ObservableCollection<DiaFiltro>
            {
                new DiaFiltro { Letra = "L", NombreCompleto = "Lunes", IsSelected = false },
                new DiaFiltro { Letra = "M", NombreCompleto = "Martes", IsSelected = false },
                new DiaFiltro { Letra = "M", NombreCompleto = "Miércoles", IsSelected = true }, // Miércoles seleccionado por defecto
                new DiaFiltro { Letra = "J", NombreCompleto = "Jueves", IsSelected = false },
                new DiaFiltro { Letra = "V", NombreCompleto = "Viernes", IsSelected = false }
            };

            SeleccionarDiaCommand = new Command<DiaFiltro>(SeleccionarDia);

            SeleccionarDia(DiasSemana[2]);
        }

        private void SeleccionarDia(DiaFiltro diaSeleccionado)
        {
            if (diaSeleccionado == null) return;

            foreach (var dia in DiasSemana)
            {
                dia.IsSelected = (dia == diaSeleccionado);
            }

            DiaActualTexto = diaSeleccionado.NombreCompleto;

          
            CargarClasesSimuladas(diaSeleccionado.NombreCompleto);
        }

        private void CargarClasesSimuladas(string dia)
        {
            ClasesDelDia.Clear();

            if (dia == "Miércoles")
            {
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Bases de Datos", Profesor = "Dra. María López Ramírez", HoraInicio = "8:00", HoraFin = "10:00", Salon = "Edificio A - 205", Acento = Color.FromArgb("#2563EB") });
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Bases de Datos", Profesor = "Dra. María López Ramírez", HoraInicio = "10:00", HoraFin = "11:00", Salon = "Edificio A - 205", Acento = Color.FromArgb("#2563EB") });
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Redes de Computadoras", Profesor = "Ing. Carlos Sánchez Torres", HoraInicio = "11:00", HoraFin = "13:00", Salon = "Edificio C - 102", Acento = Color.FromArgb("#16A34A") });
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Inteligencia Artificial", Profesor = "Dr. Roberto Méndez Pérez", HoraInicio = "13:00", HoraFin = "14:00", Salon = "Edificio B - 301", Acento = Color.FromArgb("#F59E0B") });
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Redes de Computadoras", Profesor = "Ing. Carlos Sánchez Torres", HoraInicio = "14:00", HoraFin = "15:00", Salon = "Edificio C - 102", Acento = Color.FromArgb("#16A34A") });
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Inteligencia Artificial", Profesor = "Dr. Roberto Méndez Pérez", HoraInicio = "15:00", HoraFin = "16:00", Salon = "Edificio B - 301", Acento = Color.FromArgb("#F59E0B") });
            }
            else
            {
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Día Libre o Diferente", Profesor = "Disfruta tu día", HoraInicio = "00:00", HoraFin = "23:59", Salon = "Casa", Acento = Color.FromArgb("#6B7280") });
            }
        }
    }
}