using Xunit;
using MauiApp1.src.Presentation.ViewModels.Student;

namespace MauiApp1.Tests.ViewModels.Student;

public class AsistenciaViewModelTests
{
    // ── Datos iniciales ───────────────────────────────────

    [Fact]
    public void Constructor_FechaHoyNoDebeEstarVacia()
    {
        var vm = new AsistenciaViewModel();
        Assert.False(string.IsNullOrEmpty(vm.FechaHoy));
    }

    [Fact]
    public void Constructor_HoraActualNoDebeEstarVacia()
    {
        var vm = new AsistenciaViewModel();
        Assert.False(string.IsNullOrEmpty(vm.HoraActual));
    }

    [Fact]
    public void Constructor_EstadoAsistenciaDebeEstarDefinido()
    {
        var vm = new AsistenciaViewModel();
        Assert.False(string.IsNullOrEmpty(vm.EstadoAsistencia));
    }

    [Fact]
    public void Constructor_EstadoAsistenciaDebeIndicarRegistro()
    {
        var vm = new AsistenciaViewModel();
        Assert.Contains("Asistencia", vm.EstadoAsistencia);
    }

    // ── Clase actual ──────────────────────────────────────

    [Fact]
    public void Constructor_ClaseActualNoDebeSerNull()
    {
        var vm = new AsistenciaViewModel();
        Assert.NotNull(vm.ClaseActual);
    }

    [Fact]
    public void Constructor_ClaseActualDebeTenerNombre()
    {
        var vm = new AsistenciaViewModel();
        Assert.False(string.IsNullOrEmpty(vm.ClaseActual.Nombre));
    }

    [Fact]
    public void Constructor_ClaseActualDebeTenerProfesor()
    {
        var vm = new AsistenciaViewModel();
        Assert.False(string.IsNullOrEmpty(vm.ClaseActual.Profesor));
    }

    [Fact]
    public void Constructor_ClaseActualPorcentajeDebeEstarEnRangoValido()
    {
        var vm = new AsistenciaViewModel();
        Assert.InRange(vm.ClaseActual.Porcentaje, 0, 100);
    }

    // ── Próxima clase ─────────────────────────────────────

    [Fact]
    public void Constructor_ProximaClaseNoDebeSerNull()
    {
        var vm = new AsistenciaViewModel();
        Assert.NotNull(vm.ProximaClase);
    }

    [Fact]
    public void Constructor_ProximaClaseDebeTenerNombre()
    {
        var vm = new AsistenciaViewModel();
        Assert.False(string.IsNullOrEmpty(vm.ProximaClase.Nombre));
    }

    // ── Comandos ──────────────────────────────────────────

    [Fact]
    public void Constructor_EscanearCommandNoDebeSerNull()
    {
        var vm = new AsistenciaViewModel();
        Assert.NotNull(vm.EscanearCommand);
    }

    [Fact]
    public void Constructor_ManualCommandNoDebeSerNull()
    {
        var vm = new AsistenciaViewModel();
        Assert.NotNull(vm.ManualCommand);
    }

    [Fact]
    public void EscanearCommand_DebePoderEjecutarse()
    {
        var vm = new AsistenciaViewModel();
        var excepcion = Record.Exception(() => vm.EscanearCommand.Execute(null));
        Assert.Null(excepcion);
    }

    [Fact]
    public void ManualCommand_DebePoderEjecutarse()
    {
        var vm = new AsistenciaViewModel();
        var excepcion = Record.Exception(() => vm.ManualCommand.Execute(null));
        Assert.Null(excepcion);
    }
}
