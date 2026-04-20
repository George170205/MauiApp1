using MauiApp1.src.Presentation.ViewModels.Student;

namespace MauiApp1.src.Presentation.Views
{
    public partial class PerfilAlumnoPage : ContentPage
    {
        public PerfilAlumnoPage()
        {
            InitializeComponent();
            BindingContext = new PerfilAlumnoViewModel();
        }
    }
}