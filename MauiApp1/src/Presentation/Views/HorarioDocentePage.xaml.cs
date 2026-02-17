namespace MauiApp1.src.Presentation.Views;


public partial class HorarioDocentePage : ContentPage
{
    private readonly DocenteInfo _docente;
    private List<HorarioClase> _horarios;

    private readonly string[] _dias = { "", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes" };
    private readonly string[] _horas = {
        "07:00-08:00",
        "08:00-09:00",
        "09:00-10:00",
        "10:00-11:00",
        "11:00-12:00",
        "12:00-13:00",
        "13:00-14:00",
        "14:00-15:00"
    };

    public HorarioDocentePage(DocenteInfo docente)
    {
        InitializeComponent();
        _docente = docente;

        lblNombreDocente.Text = $"{docente.Nombre} {docente.Apellido}";
        lblEspecialidad.Text = docente.Especialidad;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarHorario();
    }

    private async Task CargarHorario()
    {
        try
        {
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;
            tablaHorario.IsVisible = false;

            // Simulación de datos - reemplazar con llamada a la API
            _horarios = GenerarHorarioEjemplo();

            CrearTablaHorario();

            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            tablaHorario.IsVisible = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo cargar el horario: {ex.Message}", "OK");
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private List<HorarioClase> GenerarHorarioEjemplo()
    {
        // Ejemplo de horario - reemplazar con datos reales de la API
        return new List<HorarioClase>
        {
            new HorarioClase { Dia = "Lunes", HoraInicio = "08:00", HoraFin = "09:00", Grupo = "3-A", Materia = "Matemáticas" },
            new HorarioClase { Dia = "Lunes", HoraInicio = "10:00", HoraFin = "11:00", Grupo = "3-B", Materia = "Matemáticas" },
            new HorarioClase { Dia = "Martes", HoraInicio = "09:00", HoraFin = "10:00", Grupo = "3-A", Materia = "Álgebra" },
            new HorarioClase { Dia = "Miércoles", HoraInicio = "08:00", HoraFin = "09:00", Grupo = "3-C", Materia = "Geometría" },
            new HorarioClase { Dia = "Jueves", HoraInicio = "11:00", HoraFin = "12:00", Grupo = "3-B", Materia = "Cálculo" },
            new HorarioClase { Dia = "Viernes", HoraInicio = "13:00", HoraFin = "14:00", Grupo = "3-A", Materia = "Estadística" }
        };
    }

    private void CrearTablaHorario()
    {
        tablaHorario.Children.Clear();

        // Encabezado con días de la semana
        var headerGrid = new Grid
        {
            ColumnSpacing = 0,
            BackgroundColor = Color.FromArgb("#F3F4F6"),
            Padding = new Thickness(0)
        };

        // Definir columnas
        headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) });
        foreach (var dia in _dias.Skip(1))
        {
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120, GridUnitType.Absolute) });
        }

        // Agregar encabezados
        for (int i = 0; i < _dias.Length; i++)
        {
            var border = new Border
            {
                StrokeThickness = 1,
                Stroke = Color.FromArgb("#E5E7EB"),
                Padding = new Thickness(10),
                BackgroundColor = Color.FromArgb("#1967D2")
            };

            var label = new Label
            {
                Text = _dias[i],
                FontFamily = "InterBold",
                FontSize = 13,
                FontAttributes = FontAttributes.Bold,
                TextColor = i == 0 ? Color.FromArgb("#1F2937") : Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            if (i == 0)
                border.BackgroundColor = Color.FromArgb("#F3F4F6");

            border.Content = label;
            Grid.SetColumn(border, i);
            headerGrid.Children.Add(border);
        }

        tablaHorario.Children.Add(headerGrid);

        // Crear filas de horarios
        foreach (var hora in _horas)
        {
            var filaGrid = new Grid
            {
                ColumnSpacing = 0
            };

            // Definir columnas
            filaGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) });
            foreach (var dia in _dias.Skip(1))
            {
                filaGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120, GridUnitType.Absolute) });
            }

            // Columna de hora
            var horaBorder = new Border
            {
                StrokeThickness = 1,
                Stroke = Color.FromArgb("#E5E7EB"),
                Padding = new Thickness(8),
                BackgroundColor = Color.FromArgb("#F9FAFB"),
                MinimumHeightRequest = 70
            };

            var horaLabel = new Label
            {
                Text = hora,
                FontSize = 11,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#6B7280"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            horaBorder.Content = horaLabel;
            Grid.SetColumn(horaBorder, 0);
            filaGrid.Children.Add(horaBorder);

            // Celdas de cada día
            for (int diaIndex = 1; diaIndex < _dias.Length; diaIndex++)
            {
                var dia = _dias[diaIndex];
                var horaInicio = hora.Split('-')[0];

                // Buscar si hay una clase en este horario
                var clase = _horarios.FirstOrDefault(h =>
                    h.Dia == dia && h.HoraInicio == horaInicio);

                var celdaBorder = new Border
                {
                    StrokeThickness = 1,
                    Stroke = Color.FromArgb("#E5E7EB"),
                    Padding = new Thickness(8),
                    BackgroundColor = clase != null ? Color.FromArgb("#DBEAFE") : Colors.White,
                    MinimumHeightRequest = 70
                };

                if (clase != null)
                {
                    var claseLayout = new VerticalStackLayout
                    {
                        Spacing = 3,
                        VerticalOptions = LayoutOptions.Center
                    };

                    var grupoLabel = new Label
                    {
                        Text = clase.Grupo,
                        FontFamily = "InterBold",
                        FontSize = 13,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.FromArgb("#1967D2"),
                        HorizontalOptions = LayoutOptions.Center
                    };

                    var materiaLabel = new Label
                    {
                        Text = clase.Materia,
                        FontSize = 11,
                        TextColor = Color.FromArgb("#374151"),
                        HorizontalOptions = LayoutOptions.Center,
                        MaxLines = 2,
                        LineBreakMode = LineBreakMode.TailTruncation
                    };

                    claseLayout.Children.Add(grupoLabel);
                    claseLayout.Children.Add(materiaLabel);

                    celdaBorder.Content = claseLayout;

                    // Agregar gesto de tap para eliminar clase
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += async (s, e) =>
                    {
                        await MostrarOpcionesClase(clase);
                    };
                    celdaBorder.GestureRecognizers.Add(tapGesture);
                }
                else
                {
                    var emptyLabel = new Label
                    {
                        Text = "─",
                        FontSize = 16,
                        TextColor = Color.FromArgb("#D1D5DB"),
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    };

                    celdaBorder.Content = emptyLabel;

                    // Gesto para agregar clase
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += async (s, e) =>
                    {
                        await AgregarClaseEnHorario(dia, horaInicio);
                    };
                    celdaBorder.GestureRecognizers.Add(tapGesture);
                }

                Grid.SetColumn(celdaBorder, diaIndex);
                filaGrid.Children.Add(celdaBorder);
            }

            tablaHorario.Children.Add(filaGrid);
        }
    }

    private async Task MostrarOpcionesClase(HorarioClase clase)
    {
        var action = await DisplayActionSheet(
            $"{clase.Grupo} - {clase.Materia}",
            "Cancelar",
            "🗑️ Eliminar",
            "✏️ Editar",
            "👥 Ver Alumnos");

        switch (action)
        {
            case "🗑️ Eliminar":
                await EliminarClase(clase);
                break;
            case "✏️ Editar":
                await DisplayAlert("Editar", "Funcionalidad de edición próximamente...", "OK");
                break;
            case "👥 Ver Alumnos":
                await DisplayAlert("Alumnos", $"Ver lista de alumnos del grupo {clase.Grupo}", "OK");
                break;
        }
    }

    private async Task EliminarClase(HorarioClase clase)
    {
        bool confirmar = await DisplayAlert(
            "Confirmar Eliminación",
            $"¿Deseas eliminar la clase de {clase.Materia} del grupo {clase.Grupo}?",
            "Sí",
            "No");

        if (confirmar)
        {
            _horarios.Remove(clase);
            CrearTablaHorario();
            await DisplayAlert("Éxito", "Clase eliminada correctamente", "OK");
        }
    }

    private async Task AgregarClaseEnHorario(string dia, string horaInicio)
    {
        await DisplayAlert("Agregar Clase",
            $"Asignar nueva clase para {dia} a las {horaInicio}\nFuncionalidad próximamente...",
            "OK");
    }

    private async void OnAsignarGrupoClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Asignar Grupo",
            "Selecciona un grupo y horario disponible para asignar al docente.\nToca una celda vacía en la tabla para asignar.",
            "Entendido");
    }

    private async void OnVerGruposClicked(object sender, EventArgs e)
    {
        var gruposAsignados = _horarios.Select(h => h.Grupo).Distinct().ToList();

        if (gruposAsignados.Count == 0)
        {
            await DisplayAlert("Sin Grupos", "Este docente no tiene grupos asignados aún.", "OK");
            return;
        }

        string mensaje = "Grupos asignados:\n\n" + string.Join("\n", gruposAsignados.Select(g => $"• {g}"));
        await DisplayAlert("Grupos del Docente", mensaje, "OK");
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

// Clase auxiliar para horarios
public class HorarioClase
{
    public string Dia { get; set; }
    public string HoraInicio { get; set; }
    public string HoraFin { get; set; }
    public string Grupo { get; set; }
    public string Materia { get; set; }
}
