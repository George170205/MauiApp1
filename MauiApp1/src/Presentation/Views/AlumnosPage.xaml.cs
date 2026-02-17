//<!--AlumnosPage.xaml.cs-->
using MauiApp1.Services;
using MauiApp1.src.Core.Models;
using Microsoft.Maui.Controls.Shapes;

namespace MauiApp1.src.Presentation.Views;


public partial class AlumnosPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();
    private List<AlumnoSimple> _todosLosAlumnos = new List<AlumnoSimple>();
    private List<AlumnoSimple> _alumnosFiltrados = new List<AlumnoSimple>();
    private string _filtroActual = "Todos";

    public AlumnosPage()
    {
        InitializeComponent();
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

            // Convertir los alumnos de la API a AlumnoSimple
            var alumnosAPI = await _apiService.GetAlumnos();
            _todosLosAlumnos = alumnosAPI.Select(a => new AlumnoSimple
            {
                AlumnoID = a.AlumnoID,
                Nombre = a.Nombre,
                Apellido = a.Apellido,
                Matricula = a.Matricula,
                Email = a.Email,
                Telefono = a.Telefono,
                Activo = a.Activo,
                UsuarioID = a.UsuarioID
            }).ToList();

            AplicarFiltro();
            ActualizarContador();

            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            listaAlumnos.IsVisible = true;
        }
        catch (Exception ex)
        {
            // Si falla la API, usar datos de ejemplo
            _todosLosAlumnos = GenerarAlumnosEjemplo();
            AplicarFiltro();
            ActualizarContador();

            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            listaAlumnos.IsVisible = true;
        }
    }

    private List<AlumnoSimple> GenerarAlumnosEjemplo()
    {
        var alumnos = new List<AlumnoSimple>();
        var nombres = new[] { "Juan", "María", "Carlos", "Ana", "Pedro", "Laura", "José", "Carmen", "Luis", "Sofia" };
        var apellidos = new[] { "García", "López", "Martínez", "Rodríguez", "González", "Hernández", "Pérez", "Sánchez", "Ramírez", "Torres" };

        for (int i = 0; i < 20; i++)
        {
            var random = new Random(i + 100);
            var nombre = nombres[random.Next(nombres.Length)];
            var apellido = apellidos[random.Next(apellidos.Length)];

            alumnos.Add(new AlumnoSimple
            {
                AlumnoID = i + 1,
                Nombre = nombre,
                Apellido = apellido,
                Matricula = $"2024{(i + 1):D4}",
                Email = $"{nombre.ToLower()}.{apellido.ToLower()}@estudiante.edu",
                Telefono = $"664{random.Next(1000000, 9999999)}",
                Activo = random.Next(0, 10) > 2, // 80% activos
                UsuarioID = i + 1
            });
        }

        return alumnos.OrderBy(a => a.Apellido).ThenBy(a => a.Nombre).ToList();
    }

    private void AplicarFiltro()
    {
        string searchText = searchEntry.Text?.ToLower() ?? "";

        _alumnosFiltrados = _todosLosAlumnos.Where(a =>
        {
            bool matchesSearch = string.IsNullOrEmpty(searchText) ||
                                a.Nombre.ToLower().Contains(searchText) ||
                                a.Apellido.ToLower().Contains(searchText) ||
                                a.Email.ToLower().Contains(searchText) ||
                                a.Matricula.ToLower().Contains(searchText);

            bool matchesStatus = _filtroActual == "Todos" ||
                                (_filtroActual == "Activos" && a.Activo) ||
                                (_filtroActual == "Inactivos" && !a.Activo);

            return matchesSearch && matchesStatus;
        }).ToList();

        MostrarAlumnos();
    }

    private void MostrarAlumnos()
    {
        listaAlumnos.Children.Clear();

        if (_alumnosFiltrados.Count == 0)
        {
            noResultsView.IsVisible = true;
            return;
        }

        noResultsView.IsVisible = false;

        foreach (var alumno in _alumnosFiltrados)
        {
            var card = CrearTarjetaAlumno(alumno);
            listaAlumnos.Children.Add(card);
        }
    }

    private Border CrearTarjetaAlumno(AlumnoSimple alumno)
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
                new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(60, GridUnitType.Absolute) }
            },
            ColumnSpacing = 12
        };

        // Avatar con iniciales
        var avatarBorder = new Border
        {
            WidthRequest = 50,
            HeightRequest = 50,
            BackgroundColor = alumno.Activo ? Color.FromArgb("#1967D2") : Color.FromArgb("#9CA3AF"),
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 25 },
            VerticalOptions = LayoutOptions.Center
        };

        var iniciales = $"{alumno.Nombre[0]}{alumno.Apellido[0]}".ToUpper();
        var avatarLabel = new Label
        {
            Text = iniciales,
            FontFamily = "InterBold",
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        avatarBorder.Content = avatarLabel;
        Grid.SetColumn(avatarBorder, 0);
        mainLayout.Children.Add(avatarBorder);

        // Información del alumno
        var infoLayout = new VerticalStackLayout
        {
            Spacing = 4,
            VerticalOptions = LayoutOptions.Center
        };

        var nombreLabel = new Label
        {
            Text = $"{alumno.Nombre} {alumno.Apellido}",
            FontFamily = "InterBold",
            FontSize = 15,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#1F2937")
        };

        var matriculaLabel = new Label
        {
            Text = $"📚 {alumno.Matricula}",
            FontSize = 12,
            TextColor = Color.FromArgb("#6B7280")
        };

        var emailLabel = new Label
        {
            Text = $"✉️ {alumno.Email}",
            FontSize = 12,
            TextColor = Color.FromArgb("#6B7280")
        };

        infoLayout.Children.Add(nombreLabel);
        infoLayout.Children.Add(matriculaLabel);
        infoLayout.Children.Add(emailLabel);

        Grid.SetColumn(infoLayout, 1);
        mainLayout.Children.Add(infoLayout);

        // Switch de estado
        var switchLayout = new VerticalStackLayout
        {
            Spacing = 4,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.End
        };

        var estadoLabel = new Label
        {
            Text = alumno.Activo ? "Activo" : "Inactivo",
            FontSize = 11,
            FontAttributes = FontAttributes.Bold,
            TextColor = alumno.Activo ? Color.FromArgb("#10B981") : Color.FromArgb("#EF4444"),
            HorizontalOptions = LayoutOptions.Center
        };

        var statusSwitch = new Switch
        {
            IsToggled = alumno.Activo,
            OnColor = Color.FromArgb("#10B981"),
            ThumbColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center
        };

        statusSwitch.Toggled += async (s, e) =>
        {
            await CambiarEstadoAlumno(alumno, e.Value, estadoLabel);
        };

        switchLayout.Children.Add(estadoLabel);
        switchLayout.Children.Add(statusSwitch);

        Grid.SetColumn(switchLayout, 2);
        mainLayout.Children.Add(switchLayout);

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

    private async Task CambiarEstadoAlumno(AlumnoSimple alumno, bool nuevoEstado, Label estadoLabel)
    {
        try
        {
            alumno.Activo = nuevoEstado;
            estadoLabel.Text = nuevoEstado ? "Activo" : "Inactivo";
            estadoLabel.TextColor = nuevoEstado ? Color.FromArgb("#10B981") : Color.FromArgb("#EF4444");

            await DisplayAlert("Éxito",
                $"Estado de {alumno.Nombre} actualizado a {(nuevoEstado ? "Activo" : "Inactivo")}",
                "OK");

            ActualizarContador();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo actualizar el estado: {ex.Message}", "OK");
        }
    }

    private async Task MostrarDetallesAlumno(AlumnoSimple alumno)
    {
        await DisplayAlert("Detalles del Alumno",
            $"Nombre: {alumno.Nombre} {alumno.Apellido}\n" +
            $"Matrícula: {alumno.Matricula}\n" +
            $"Email: {alumno.Email}\n" +
            $"Teléfono: {alumno.Telefono ?? "N/A"}\n" +
            $"Estado: {(alumno.Activo ? "Activo" : "Inactivo")}",
            "OK");
    }

    private void ActualizarContador()
    {
        int activos = _todosLosAlumnos.Count(a => a.Activo);
        int total = _todosLosAlumnos.Count;
        lblTotalAlumnos.Text = $"{total} alumnos ({activos} activos, {total - activos} inactivos)";
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        AplicarFiltro();
    }

    private void OnFiltroTodosClicked(object sender, EventArgs e)
    {
        _filtroActual = "Todos";
        ActualizarBotonesFiltro();
        AplicarFiltro();
    }

    private void OnFiltroActivosClicked(object sender, EventArgs e)
    {
        _filtroActual = "Activos";
        ActualizarBotonesFiltro();
        AplicarFiltro();
    }

    private void OnFiltroInactivosClicked(object sender, EventArgs e)
    {
        _filtroActual = "Inactivos";
        ActualizarBotonesFiltro();
        AplicarFiltro();
    }

    private void ActualizarBotonesFiltro()
    {
        btnTodos.BackgroundColor = Color.FromArgb("#E5E7EB");
        btnActivos.BackgroundColor = Color.FromArgb("#E5E7EB");
        btnInactivos.BackgroundColor = Color.FromArgb("#E5E7EB");

        var todosLabel = (Label)btnTodos.Content;
        var activosLabel = (Label)btnActivos.Content;
        var inactivosLabel = (Label)btnInactivos.Content;

        todosLabel.TextColor = Color.FromArgb("#6B7280");
        activosLabel.TextColor = Color.FromArgb("#6B7280");
        inactivosLabel.TextColor = Color.FromArgb("#6B7280");

        switch (_filtroActual)
        {
            case "Todos":
                btnTodos.BackgroundColor = Color.FromArgb("#1967D2");
                todosLabel.TextColor = Colors.White;
                break;
            case "Activos":
                btnActivos.BackgroundColor = Color.FromArgb("#10B981");
                activosLabel.TextColor = Colors.White;
                break;
            case "Inactivos":
                btnInactivos.BackgroundColor = Color.FromArgb("#EF4444");
                inactivosLabel.TextColor = Colors.White;
                break;
        }
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnAgregarAlumnoClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Agregar Alumno",
            "Funcionalidad para agregar nuevo alumno.\nPróximamente...",
            "OK");
    }
}
