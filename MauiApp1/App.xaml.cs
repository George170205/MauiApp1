namespace MauiApp1;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        System.Diagnostics.Debug.WriteLine(" CICLO DE VIDA: Aplicación Iniciada");
    }

    protected override void OnSleep()
    {
        System.Diagnostics.Debug.WriteLine(" CICLO DE VIDA: En Segundo Plano");
    }

    protected override void OnResume()
    {
        System.Diagnostics.Debug.WriteLine(" CICLO DE VIDA: Resumida");
    }
}