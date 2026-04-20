using Xunit;
using MauiApp1.src.Presentation.ViewModels.Student;

namespace MauiApp1.Tests.ViewModels.Student;

public class HomeAlumnoViewModelTests
{
    // ── Datos iniciales ───────────────────────────────────

    [Fact]
    public void Constructor_NombreDebeEstarDefinido()
    {
        var vm = new HomeAlumnoViewModel();
        Assert.False(string.IsNullOrEmpty(vm.Nombre));
    }

    [Fact]
    public void Constructor_InicialesDebenCorresponderAlNombre()
    {
        var vm = new HomeAlumnoViewModel();
        Assert.Equal("JR", vm.Iniciales);
    }

    [Fact]
    public void Constructor_AsistenciasDebenEstarEnRangoValido()
    {
        var vm = new HomeAlumnoViewModel();
        Assert.InRange(vm.Asistencias, 0, 100);
    }

    [Fact]
    public void Constructor_PromedioDebeEstarEnRangoValido()
    {
        var vm = new HomeAlumnoViewModel();
        Assert.InRange(vm.Promedio, 0.0, 10.0);
    }

    [Fact]
    public void Constructor_MateriasDebeSerMayorACero()
    {
        var vm = new HomeAlumnoViewModel();
        Assert.True(vm.Materias > 0);
    }

    // ── Próxima clase ─────────────────────────────────────

    [Fact]
    public void Constructor_ProximaClaseNoDebeSerNull()
    {
        var vm = new HomeAlumnoViewModel();
        Assert.NotNull(vm.ProximaClase);
    }

    [Fact]
    public void Constructor_ProximaClaseDebeTenerNombre()
    {
        var vm = new HomeAlumnoViewModel();
        Assert.False(string.IsNullOrEmpty(vm.ProximaClase.Nombre));
    }

    // ── Lista de materias ─────────────────────────────────

    [Fact]
    public void Constructor_MateriasListNoDebeEstarVacia()
    {
        var vm = new HomeAlumnoViewModel();
        Assert.NotEmpty(vm.MateriasList);
    }

    [Fact]
    public void Constructor_TodasLasMateriasDebenTenerNombre()
    {
        var vm = new HomeAlumnoViewModel();
        Assert.All(vm.MateriasList, m => Assert.False(string.IsNullOrEmpty(m.Nombre)));
    }

    [Fact]
    public void Constructor_TodasLasMateriasDebenTenerPorcentajeValido()
    {
        var vm = new HomeAlumnoViewModel();
        Assert.All(vm.MateriasList, m => Assert.InRange(m.Porcentaje, 0, 100));
    }
}
