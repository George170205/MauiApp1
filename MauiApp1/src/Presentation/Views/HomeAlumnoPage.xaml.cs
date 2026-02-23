namespace MauiApp1.src.Presentation.Views;

using MauiApp1.src.Presentation.ViewModels;

public partial class HomeAlumnoPage : ContentPage
{
    public HomeAlumnoPage()
    {
        InitializeComponent();
        BindingContext = new HomeAlumnoViewModel();
    }
}