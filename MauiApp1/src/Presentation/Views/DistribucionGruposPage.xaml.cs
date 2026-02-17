using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
namespace MauiApp1.src.Presentation.Views;


public partial class DistribucionGruposPage : ContentPage
{
    private List<GrupoDistribucion> _grupos;
    private List<Color> _colores;

    public DistribucionGruposPage()
    {
        InitializeComponent();
        InicializarColores();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDatos();
    }

    private void InicializarColores()
    {
        _colores = new List<Color>
        {
            Color.FromArgb("#1967D2"), // Azul
            Color.FromArgb("#10B981"), // Verde
            Color.FromArgb("#F59E0B"), // Naranja
            Color.FromArgb("#EF4444"), // Rojo
            Color.FromArgb("#8B5CF6"), // Morado
            Color.FromArgb("#EC4899"), // Rosa
            Color.FromArgb("#14B8A6"), // Turquesa
            Color.FromArgb("#F97316"), // Naranja oscuro
            Color.FromArgb("#06B6D4"), // Cyan
            Color.FromArgb("#84CC16")  // Lima
        };
    }

    private async Task CargarDatos()
    {
        try
        {
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;

            // Simulación de datos - reemplazar con llamada a la API
            _grupos = new List<GrupoDistribucion>
            {
                new GrupoDistribucion { Nombre = "1-A", CantidadAlumnos = 28 },
                new GrupoDistribucion { Nombre = "1-B", CantidadAlumnos = 30 },
                new GrupoDistribucion { Nombre = "2-A", CantidadAlumnos = 25 },
                new GrupoDistribucion { Nombre = "2-B", CantidadAlumnos = 27 },
                new GrupoDistribucion { Nombre = "3-A", CantidadAlumnos = 32 },
                new GrupoDistribucion { Nombre = "3-B", CantidadAlumnos = 29 },
                new GrupoDistribucion { Nombre = "3-C", CantidadAlumnos = 26 },
                new GrupoDistribucion { Nombre = "4-A", CantidadAlumnos = 24 },
                new GrupoDistribucion { Nombre = "4-B", CantidadAlumnos = 31 },
                new GrupoDistribucion { Nombre = "5-A", CantidadAlumnos = 28 }
            };

            ActualizarEstadisticas();
            CrearGraficaPastel();
            CrearLeyenda();

            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los datos: {ex.Message}", "OK");
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private void ActualizarEstadisticas()
    {
        int totalGrupos = _grupos.Count;
        int totalAlumnos = _grupos.Sum(g => g.CantidadAlumnos);
        double promedio = totalAlumnos / (double)totalGrupos;
        var grupoMasGrande = _grupos.OrderByDescending(g => g.CantidadAlumnos).First();

        lblTotalAlumnos.Text = $"{totalAlumnos} alumnos en {totalGrupos} grupos";
        lblTotalGrupos.Text = totalGrupos.ToString();
        lblTotalAlumnosStats.Text = totalAlumnos.ToString();
        lblPromedioGrupo.Text = promedio.ToString("F1");
        lblGrupoMasGrande.Text = grupoMasGrande.Nombre;
    }

    private void CrearGraficaPastel()
    {
        pieChartView.Drawable = new PieChartDrawable(_grupos, _colores, async (grupo) =>
        {
            await MostrarAlumnosGrupo(grupo);
        });
    }

    private void CrearLeyenda()
    {
        leyendaContainer.Children.Clear();

        for (int i = 0; i < _grupos.Count; i++)
        {
            var grupo = _grupos[i];
            var color = _colores[i % _colores.Count];

            var itemLayout = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(20, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(60, GridUnitType.Absolute) }
                },
                ColumnSpacing = 10
            };

            // Cuadro de color
            var colorBox = new Border
            {
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = color,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = 4 },
                VerticalOptions = LayoutOptions.Center
            };

            Grid.SetColumn(colorBox, 0);
            itemLayout.Children.Add(colorBox);

            // Nombre del grupo
            var nombreLabel = new Label
            {
                Text = grupo.Nombre,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#1F2937"),
                VerticalOptions = LayoutOptions.Center
            };

            Grid.SetColumn(nombreLabel, 1);
            itemLayout.Children.Add(nombreLabel);

            // Cantidad de alumnos
            var cantidadLabel = new Label
            {
                Text = $"{grupo.CantidadAlumnos} alumnos",
                FontSize = 12,
                TextColor = Color.FromArgb("#6B7280"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End
            };

            Grid.SetColumn(cantidadLabel, 2);
            itemLayout.Children.Add(cantidadLabel);

            // Agregar gesto de tap
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                await MostrarAlumnosGrupo(grupo);
            };
            itemLayout.GestureRecognizers.Add(tapGesture);

            leyendaContainer.Children.Add(itemLayout);
        }
    }

    private async Task MostrarAlumnosGrupo(GrupoDistribucion grupo)
    {
        // Aquí podrías navegar a una página detallada o mostrar un popup
        await Navigation.PushAsync(new AlumnosGrupoPage(grupo));
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

// Clase para dibujar el gráfico de pastel
public class PieChartDrawable : IDrawable
{
    private readonly List<GrupoDistribucion> _grupos;
    private readonly List<Color> _colores;
    private readonly Func<GrupoDistribucion, Task> _onSegmentTapped;

    public PieChartDrawable(List<GrupoDistribucion> grupos, List<Color> colores, Func<GrupoDistribucion, Task> onSegmentTapped)
    {
        _grupos = grupos;
        _colores = colores;
        _onSegmentTapped = onSegmentTapped;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float centerX = dirtyRect.Width / 2;
        float centerY = dirtyRect.Height / 2;
        float radius = Math.Min(dirtyRect.Width, dirtyRect.Height) / 2 - 20;

        int total = _grupos.Sum(g => g.CantidadAlumnos);
        float startAngle = -90;

        for (int i = 0; i < _grupos.Count; i++)
        {
            var grupo = _grupos[i];
            var color = _colores[i % _colores.Count];

            float sweepAngle = (grupo.CantidadAlumnos / (float)total) * 360;

            // Dibujar el segmento del pastel
            var path = new PathF();
            path.MoveTo(centerX, centerY);

            float startX = centerX + radius * (float)Math.Cos(startAngle * Math.PI / 180);
            float startY = centerY + radius * (float)Math.Sin(startAngle * Math.PI / 180);
            path.LineTo(startX, startY);

            path.AddArc(centerX - radius, centerY - radius, centerX + radius, centerY + radius, startAngle, startAngle + sweepAngle, true);
            path.Close();

            canvas.FillColor = color;
            canvas.FillPath(path);

            // Dibujar borde blanco
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 2;
            canvas.DrawPath(path);

            // Calcular posición para el texto (porcentaje)
            float percentage = (grupo.CantidadAlumnos / (float)total) * 100;
            float midAngle = startAngle + sweepAngle / 2;
            float textRadius = radius * 0.7f;
            float textX = centerX + textRadius * (float)Math.Cos(midAngle * Math.PI / 180);
            float textY = centerY + textRadius * (float)Math.Sin(midAngle * Math.PI / 180);

            // Dibujar porcentaje
            canvas.FontColor = Colors.White;
            canvas.FontSize = 12;
            canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;
            string text = $"{percentage:F1}%";
            canvas.DrawString(text, textX - 20, textY - 6, 40, 20, HorizontalAlignment.Center, VerticalAlignment.Center);

            startAngle += sweepAngle;
        }

        // Dibujar círculo blanco en el centro para efecto "donut" (opcional)
        float innerRadius = radius * 0.5f;
        canvas.FillColor = Color.FromArgb("#F9FAFB");
        canvas.FillCircle(centerX, centerY, innerRadius);
    }
}
