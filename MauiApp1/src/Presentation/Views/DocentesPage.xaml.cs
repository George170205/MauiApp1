using MauiApp1.Services;
using MauiApp1.src.Core.Models;
using Microsoft.Maui.Controls.Shapes;

namespace MauiApp1.src.Presentation.Views;

public partial class DocentesPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public DocentesPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDocentes();
    }

    private async Task CargarDocentes()
    {
        try
        {
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;
            listaDocentes.IsVisible = false;

            // Simulación de datos - reemplazar con llamada real a la API
            var docentes = new List<DocenteInfo>
            {
                new DocenteInfo
                {
                    DocenteID = 1,
                    Nombre = "Dr. Carlos",
                    Apellido = "Ramírez González",
                    Email = "carlos.ramirez@escuela.edu",
                    Especialidad = "Matemáticas",
                    GruposAsignados = 4
                },
                new DocenteInfo
                {
                    DocenteID = 2,
                    Nombre = "Dra. María",
                    Apellido = "López Hernández",
                    Email = "maria.lopez@escuela.edu",
                    Especialidad = "Física",
                    GruposAsignados = 3
                },
                new DocenteInfo
                {
                    DocenteID = 3,
                    Nombre = "Lic. Juan",
                    Apellido = "Martínez Silva",
                    Email = "juan.martinez@escuela.edu",
                    Especialidad = "Química",
                    GruposAsignados = 5
                },
                new DocenteInfo
                {
                    DocenteID = 4,
                    Nombre = "Mtro. Pedro",
                    Apellido = "García Torres",
                    Email = "pedro.garcia@escuela.edu",
                    Especialidad = "Historia",
                    GruposAsignados = 0
                }
            };

            MostrarDocentes(docentes);
            lblTotalDocentes.Text = $"{docentes.Count} docentes registrados";

            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            listaDocentes.IsVisible = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los docentes: {ex.Message}", "OK");
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private void MostrarDocentes(List<DocenteInfo> docentes)
    {
        listaDocentes.Children.Clear();

        foreach (var docente in docentes)
        {
            var card = CrearTarjetaDocente(docente);
            listaDocentes.Children.Add(card);
        }
    }

    private Border CrearTarjetaDocente(DocenteInfo docente)
    {
        var card = new Border
        {
            BackgroundColor = Colors.White,
            Padding = new Thickness(15),
            StrokeThickness = 1,
            Stroke = Color.FromArgb("#E5E7EB"),
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Shadow = new Shadow
            {
                Brush = Colors.Black,
                Offset = new Point(0, 2),
                Radius = 8,
                Opacity = 0.06f
            }
        };

        var mainLayout = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(60, GridUnitType.Absolute) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(40, GridUnitType.Absolute) }
            },
            ColumnSpacing = 12
        };

        // Avatar con iniciales
        var avatarBorder = new Border
        {
            WidthRequest = 60,
            HeightRequest = 60,
            BackgroundColor = Color.FromArgb("#1967D2"),
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 30 },
            VerticalOptions = LayoutOptions.Center
        };

        var iniciales = $"{docente.Nombre[0]}{docente.Apellido[0]}".ToUpper();
        var avatarLabel = new Label
        {
            Text = iniciales,
            FontFamily = "InterBold",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        avatarBorder.Content = avatarLabel;
        Grid.SetColumn(avatarBorder, 0);
        mainLayout.Children.Add(avatarBorder);

        // Información del docente
        var infoLayout = new VerticalStackLayout
        {
            Spacing = 4,
            VerticalOptions = LayoutOptions.Center
        };

        var nombreLabel = new Label
        {
            Text = $"{docente.Nombre} {docente.Apellido}",
            FontFamily = "InterBold",
            FontSize = 15,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#1F2937")
        };

        var especialidadLabel = new Label
        {
            Text = $"📚 {docente.Especialidad}",
            FontSize = 12,
            TextColor = Color.FromArgb("#6B7280")
        };

        var emailLabel = new Label
        {
            Text = $"✉️ {docente.Email}",
            FontSize = 12,
            TextColor = Color.FromArgb("#6B7280")
        };

        var gruposLabel = new Label
        {
            Text = $"👥 {docente.GruposAsignados} grupos asignados",
            FontSize = 12,
            FontAttributes = FontAttributes.Bold,
            TextColor = docente.GruposAsignados == 0 ? Color.FromArgb("#EF4444") : Color.FromArgb("#10B981")
        };

        infoLayout.Children.Add(nombreLabel);
        infoLayout.Children.Add(especialidadLabel);
        infoLayout.Children.Add(emailLabel);
        infoLayout.Children.Add(gruposLabel);

        Grid.SetColumn(infoLayout, 1);
        mainLayout.Children.Add(infoLayout);

        // Icono de navegación
        var chevron = new Label
        {
            Text = "›",
            FontSize = 30,
            TextColor = Color.FromArgb("#9CA3AF"),
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };

        Grid.SetColumn(chevron, 2);
        mainLayout.Children.Add(chevron);

        card.Content = mainLayout;

        // Agregar gesto de tap para ver horario
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += async (s, e) =>
        {
            await Navigation.PushAsync(new HorarioDocentePage(docente));
        };
        card.GestureRecognizers.Add(tapGesture);

        return card;
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

// Clase auxiliar para información de docentes
public class DocenteInfo
{
    public int DocenteID { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Especialidad { get; set; }
    public int GruposAsignados { get; set; }
}
