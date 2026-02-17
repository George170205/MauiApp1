using MauiApp1.Services;
using MauiApp1.src.Core.Models;
using Microsoft.Maui.Controls.Shapes;

namespace MauiApp1.src.Presentation.Views;

public partial class AlumnosGrupoPage : ContentPage
{
    private readonly GrupoDistribucion _grupo;
    private List<AlumnoSimple> _todosLosAlumnos = new List<AlumnoSimple>();
    private List<AlumnoSimple> _alumnosFiltrados = new List<AlumnoSimple>();

    public AlumnosGrupoPage(GrupoDistribucion grupo)
    {
        InitializeComponent();
        _grupo = grupo;

        lblNombreGrupo.Text = $"Grupo {grupo.Nombre}";
        lblCantidadAlumnos.Text = $"{grupo.CantidadAlumnos} alumnos inscritos";
        lblTotalAlumnos.Text = grupo.CantidadAlumnos.ToString();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarAlumnos();
    }

    private async Task CargarAlumnos()
    {
        try
        {
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;
            listaAlumnos.IsVisible = false;

            _todosLosAlumnos = GenerarAlumnosEjemplo();
            _alumnosFiltrados = _todosLosAlumnos;

            MostrarAlumnos();

            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            listaAlumnos.IsVisible = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los alumnos: {ex.Message}", "OK");
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private List<AlumnoSimple> GenerarAlumnosEjemplo()
    {
        var nombres = new[] { "Juan", "María", "Carlos", "Ana", "Pedro", "Laura", "José", "Carmen", "Luis", "Sofia" };
        var apellidos = new[] { "García", "López", "Martínez", "Rodríguez", "González", "Hernández", "Pérez", "Sánchez", "Ramírez", "Torres" };
        var alumnos = new List<AlumnoSimple>();

        for (int i = 0; i < _grupo.CantidadAlumnos; i++)
        {
            var random = new Random(i + (_grupo.Nombre.GetHashCode()));
            var nombre = nombres[random.Next(nombres.Length)];
            var apellido = apellidos[random.Next(apellidos.Length)];

            alumnos.Add(new AlumnoSimple
            {
                AlumnoID = i + 1,
                Nombre = nombre,
                Apellido = apellido,
                Matricula = $"2024{_grupo.Nombre.Replace("-", "")}{(i + 1):D3}",
                Email = $"{nombre.ToLower()}.{apellido.ToLower()}@estudiante.edu",
                Telefono = $"664{random.Next(1000000, 9999999)}",
                Activo = true,
                UsuarioID = i + 1
            });
        }

        return alumnos.OrderBy(a => a.Apellido).ThenBy(a => a.Nombre).ToList();
    }

    private void MostrarAlumnos()
    {
        listaAlumnos.Children.Clear();

        int numero = 1;
        foreach (var alumno in _alumnosFiltrados)
        {
            var card = CrearTarjetaAlumno(alumno, numero);
            listaAlumnos.Children.Add(card);
            numero++;
        }
    }

    private Border CrearTarjetaAlumno(AlumnoSimple alumno, int numero)
    {
        var card = new Border
        {
            BackgroundColor = Colors.White,
            Padding = new Thickness(12),
            StrokeThickness = 1,
            Stroke = Color.FromArgb("#E5E7EB"),
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Shadow = new Shadow
            {
                Brush = Colors.Black,
                Offset = new Point(0, 1),
                Radius = 4,
                Opacity = 0.05f
            }
        };

        var mainLayout = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(40, GridUnitType.Absolute) },
                new ColumnDefinition { Width = new GridLength(45, GridUnitType.Absolute) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            },
            ColumnSpacing = 10
        };

        // Número de lista
        var numeroBorder = new Border
        {
            WidthRequest = 30,
            HeightRequest = 30,
            BackgroundColor = Color.FromArgb("#F3F4F6"),
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 15 },
            VerticalOptions = LayoutOptions.Center
        };

        var numeroLabel = new Label
        {
            Text = numero.ToString(),
            FontFamily = "InterBold",
            FontSize = 13,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#6B7280"),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        numeroBorder.Content = numeroLabel;
        Grid.SetColumn(numeroBorder, 0);
        mainLayout.Children.Add(numeroBorder);

        // Avatar con iniciales
        var avatarBorder = new Border
        {
            WidthRequest = 45,
            HeightRequest = 45,
            BackgroundColor = Color.FromArgb("#1967D2"),
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 22 },
            VerticalOptions = LayoutOptions.Center
        };

        var iniciales = $"{alumno.Nombre[0]}{alumno.Apellido[0]}".ToUpper();
        var avatarLabel = new Label
        {
            Text = iniciales,
            FontFamily = "InterBold",
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        avatarBorder.Content = avatarLabel;
        Grid.SetColumn(avatarBorder, 1);
        mainLayout.Children.Add(avatarBorder);

        // Información del alumno
        var infoLayout = new VerticalStackLayout
        {
            Spacing = 3,
            VerticalOptions = LayoutOptions.Center
        };

        var nombreLabel = new Label
        {
            Text = $"{alumno.Nombre} {alumno.Apellido}",
            FontFamily = "InterBold",
            FontSize = 14,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#1F2937")
        };

        var matriculaLabel = new Label
        {
            Text = $"📚 {alumno.Matricula}",
            FontSize = 11,
            TextColor = Color.FromArgb("#6B7280")
        };

        infoLayout.Children.Add(nombreLabel);
        infoLayout.Children.Add(matriculaLabel);

        Grid.SetColumn(infoLayout, 2);
        mainLayout.Children.Add(infoLayout);

        card.Content = mainLayout;

        // Agregar gesto de tap para ver detalles
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += async (s, e) =>
        {
            await MostrarDetallesAlumno(alumno);
        };
        card.GestureRecognizers.Add(tapGesture);

        return card;
    }

    private async Task MostrarDetallesAlumno(AlumnoSimple alumno)
    {
        await DisplayAlert("Detalles del Alumno",
            $"Nombre: {alumno.Nombre} {alumno.Apellido}\n" +
            $"Matrícula: {alumno.Matricula}\n" +
            $"Email: {alumno.Email}\n" +
            $"Teléfono: {alumno.Telefono ?? "N/A"}\n" +
            $"Grupo: {_grupo.Nombre}",
            "OK");
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = searchEntry.Text?.ToLower() ?? "";

        _alumnosFiltrados = _todosLosAlumnos.Where(a =>
            string.IsNullOrEmpty(searchText) ||
            a.Nombre.ToLower().Contains(searchText) ||
            a.Apellido.ToLower().Contains(searchText) ||
            a.Matricula.ToLower().Contains(searchText) ||
            a.Email.ToLower().Contains(searchText)
        ).ToList();

        MostrarAlumnos();
    }

    private async void OnAgregarAlumnoClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Agregar Alumno",
            $"Funcionalidad para agregar un nuevo alumno al grupo {_grupo.Nombre}.\nPróximamente...",
            "OK");
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

// Clase para la distribución de grupos
public class GrupoDistribucion
{
    public string Nombre { get; set; } = "";
    public int CantidadAlumnos { get; set; }
}
