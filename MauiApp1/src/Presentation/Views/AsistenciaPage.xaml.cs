namespace MauiApp1.src.Presentation.Views;

using MauiApp1.src.Presentation.ViewModels;

public partial class AsistenciaPage : ContentPage
{
    public AsistenciaPage()
    {
        InitializeComponent();
        BindingContext = new AsistenciaViewModel();
    }
}