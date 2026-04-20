using Xunit;
using MauiApp1.src.Presentation.ViewModels.Teachers;

namespace MauiApp1.Tests.ViewModels.Teachers;

public class TeachersHomeViewModelTests
{
    // ── Datos iniciales ───────────────────────────────────

    [Fact]
    public void Constructor_NombreDocenteDebeEstarDefinido()
    {
        var vm = new TeachersHomeViewModel();
        Assert.False(string.IsNullOrEmpty(vm.Nombre));
    }

    [Fact]
    public void Constructor_InicialesDebenEstarDefinidas()
    {
        var vm = new TeachersHomeViewModel();
        Assert.Equal("CS", vm.Iniciales);
    }

    [Fact]
    public void Constructor_TotalAlumnosDebeSerMayorACero()
    {
        var vm = new TeachersHomeViewModel();
        Assert.True(vm.TotalAlumnos > 0);
    }

    [Fact]
    public void Constructor_TotalMateriasDebeSerMayorACero()
    {
        var vm = new TeachersHomeViewModel();
        Assert.True(vm.TotalMaterias > 0);
    }

    [Fact]
    public void Constructor_PromedioAsistenciaDebeEstarEnRangoValido()
    {
        var vm = new TeachersHomeViewModel();
        Assert.InRange(vm.PromedioAsistencia, 0.0, 100.0);
    }

    // ── Estado QR ─────────────────────────────────────────

    [Fact]
    public void Constructor_QrActivoDebeIniciarEnFalse()
    {
        var vm = new TeachersHomeViewModel();
        Assert.False(vm.QrActivo);
    }

    [Fact]
    public void Constructor_EstadoQrTextoDebeSerInactivo()
    {
        var vm = new TeachersHomeViewModel();
        Assert.Equal("QR Inactivo", vm.EstadoQrTexto);
    }

    [Fact]
    public void RegenerarQRCommand_DebeActivarQr()
    {
        var vm = new TeachersHomeViewModel();
        vm.RegenerarQRCommand.Execute(null);
        Assert.True(vm.QrActivo);
    }

    [Fact]
    public void RegenerarQRCommand_EstadoQrTextoDebeSerActivo()
    {
        var vm = new TeachersHomeViewModel();
        vm.RegenerarQRCommand.Execute(null);
        Assert.Equal("QR Activo", vm.EstadoQrTexto);
    }

    // ── Sesión ────────────────────────────────────────────

    [Fact]
    public void Constructor_SesionCerradaDebeIniciarEnFalse()
    {
        var vm = new TeachersHomeViewModel();
        Assert.False(vm.SesionCerrada);
    }

    [Fact]
    public void CerrarSesionCommand_DebeMarcarSesionComoCerrada()
    {
        var vm = new TeachersHomeViewModel();
        vm.CerrarSesionCommand.Execute(null);
        Assert.True(vm.SesionCerrada);
    }

    // ── Lista de materias ─────────────────────────────────

    [Fact]
    public void Constructor_MateriasListNoDebeEstarVacia()
    {
        var vm = new TeachersHomeViewModel();
        Assert.NotEmpty(vm.MateriasList);
    }

    [Fact]
    public void Constructor_TodasLasMateriasDebenTenerNombre()
    {
        var vm = new TeachersHomeViewModel();
        Assert.All(vm.MateriasList, m => Assert.False(string.IsNullOrEmpty(m.Nombre)));
    }

    [Fact]
    public void Constructor_TodasLasMateriasDebenTenerPorcentajeValido()
    {
        var vm = new TeachersHomeViewModel();
        Assert.All(vm.MateriasList, m => Assert.InRange(m.Porcentaje, 0, 100));
    }

    // ── Próxima clase ─────────────────────────────────────

    [Fact]
    public void Constructor_ProximaClaseNoDebeSerNull()
    {
        var vm = new TeachersHomeViewModel();
        Assert.NotNull(vm.ProximaClase);
    }

    [Fact]
    public void Constructor_ProximaClaseDebeTenerNombre()
    {
        var vm = new TeachersHomeViewModel();
        Assert.False(string.IsNullOrEmpty(vm.ProximaClase.Nombre));
    }
}
