namespace MauiApp1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var navigationPage = new NavigationPage(new LoginPage());
            NavigationPage.SetHasNavigationBar(navigationPage, false);

            MainPage = navigationPage;
        }
    }
}