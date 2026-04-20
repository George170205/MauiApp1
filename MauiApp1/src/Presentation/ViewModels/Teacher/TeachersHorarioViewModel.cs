using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiApp1.src.Core.Models;
using Microsoft.Maui.Graphics;

namespace MauiApp1.src.Presentation.ViewModels.Teachers
{
    public class DiaFiltroDocente : BindableObject
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

    public class TeachersHorarioViewModel : BindableObject
    {
        public ObservableCollection<DiaFiltroDocente> DiasSemana { get; set; }
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

        public TeachersHorarioViewModel()
        {
            ClasesDelDia = new ObservableCollection<ClaseInfo>();

            DiasSemana = new ObservableCollection<DiaFiltroDocente>
            {
                new DiaFiltroDocente { Letra = "L", NombreCompleto = "Lunes",     IsSelected = true },
                new DiaFiltroDocente { Letra = "M", NombreCompleto = "Martes",    IsSelected = false },
                new DiaFiltroDocente { Letra = "M", NombreCompleto = "Miércoles", IsSelected = false },
                new DiaFiltroDocente { Letra = "J", NombreCompleto = "Jueves",    IsSelected = false },
                new DiaFiltroDocente { Letra = "V", NombreCompleto = "Viernes",   IsSelected = false }
            };

            SeleccionarDiaCommand = new Command<DiaFiltroDocente>(SeleccionarDia);

            SeleccionarDia(DiasSemana[0]);
        }

        private void SeleccionarDia(DiaFiltroDocente diaSeleccionado)
        {
            if (diaSeleccionado == null) return;

            foreach (var dia in DiasSemana)
                dia.IsSelected = (dia == diaSeleccionado);

            DiaActualTexto = diaSeleccionado.NombreCompleto;
            CargarClasesDelDia(diaSeleccionado.NombreCompleto);
        }

        private void CargarClasesDelDia(string dia)
        {
            ClasesDelDia.Clear();

            if (dia == "Lunes" || dia == "Miércoles")
            {
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Bases de Datos", Profesor = "Ing. Carlos Sánchez", HoraInicio = "8:00", HoraFin = "10:00", Salon = "Edificio A - 205", Acento = Color.FromArgb("#2563EB") });
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Programación Avanzada", Profesor = "Ing. Carlos Sánchez", HoraInicio = "10:00", HoraFin = "12:00", Salon = "Edificio B - 301", Acento = Color.FromArgb("#F59E0B") });
            }
            else if (dia == "Martes" || dia == "Jueves")
            {
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Redes de Computadoras", Profesor = "Ing. Carlos Sánchez", HoraInicio = "14:00", HoraFin = "16:00", Salon = "Edificio C - 102", Acento = Color.FromArgb("#16A34A") });
            }
            else
            {
                ClasesDelDia.Add(new ClaseInfo { Nombre = "Sin clases", Profesor = "", HoraInicio = "00:00", HoraFin = "00:00", Salon = "", Acento = Color.FromArgb("#6B7280") });
            }
        }
    }
}
