using MauiApp1.src.Presentation.ViewModels.Student;

namespace MauiApp1.src.Presentation.Views
{
    public partial class HorarioAlumnoPage : ContentPage
    {
        public HorarioAlumnoPage()
        {
            InitializeComponent();
            BindingContext = new HorarioAlumnoViewModel();
        }
    }
}