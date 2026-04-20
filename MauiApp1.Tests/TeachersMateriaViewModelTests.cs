using Xunit;
using MauiApp1.src.Presentation.ViewModels.Teachers;
using System;   

namespace MauiApp1.Tests.ViewModels.Teachers;

public class TeachersMateriaViewModelTests
{
    // ── Datos iniciales ───────────────────────────────────

    [Fact]
    public void Constructor_NombreMateriaDebeEstarDefinido()
    {
        var vm = new TeachersMateriaViewModel();
        Assert.False(string.IsNullOrEmpty(vm.NombreMateria));
    }

    [Fact]
    public void Constructor_TotalAlumnosDebeSerMayorACero()
    {
        var vm = new TeachersMateriaViewModel();
        Assert.True(vm.TotalAlumnos > 0);
    }

    [Fact]
    public void Constructor_AlumnosPresentesNoPuedeExcederTotalAlumnos()
    {
        var vm = new TeachersMateriaViewModel();
        Assert.True(vm.AlumnosPresentes <= vm.TotalAlumnos);
    }

    // ── Porcentaje de asistencia ──────────────────────────

    [Fact]
    public void PorcentajeAsistencia_DebeCalcularseCorrectamente()
    {
        var vm = new TeachersMateriaViewModel();
        double esperado = Math.Round((double)vm.AlumnosPresentes / vm.TotalAlumnos * 100, 1);
        Assert.Equal(esperado, vm.PorcentajeAsistencia);
    }

    [Fact]
    public void PorcentajeAsistencia_DebeEstarEnRangoValido()
    {
        var vm = new TeachersMateriaViewModel();
        Assert.InRange(vm.PorcentajeAsistencia, 0.0, 100.0);
    }

    // ── Reporte ───────────────────────────────────────────

    [Fact]
    public void Constructor_ReporteGeneradoDebeIniciarEnFalse()
    {
        var vm = new TeachersMateriaViewModel();
        Assert.False(vm.ReporteGenerado);
    }

    [Fact]
    public void Constructor_MensajeReporteDebeEstarVacioAlInicio()
    {
        var vm = new TeachersMateriaViewModel();
        Assert.Equal(string.Empty, vm.MensajeReporte);
    }

    [Fact]
    public void GenerarReporteCommand_DebeMarcarReporteComoGenerado()
    {
        var vm = new TeachersMateriaViewModel();
        vm.GenerarReporteCommand.Execute(null);
        Assert.True(vm.ReporteGenerado);
    }

    [Fact]
    public void GenerarReporteCommand_MensajeDebeActualizarse()
    {
        var vm = new TeachersMateriaViewModel();
        vm.GenerarReporteCommand.Execute(null);
        Assert.False(string.IsNullOrEmpty(vm.MensajeReporte));
    }

    // ── QR ────────────────────────────────────────────────

    [Fact]
    public void Constructor_QrRegeneradoDebeIniciarEnFalse()
    {
        var vm = new TeachersMateriaViewModel();
        Assert.False(vm.QrRegenerado);
    }

    [Fact]
    public void RegenerarQRCommand_DebeMarcarQrComoRegenerado()
    {
        var vm = new TeachersMateriaViewModel();
        vm.RegenerarQRCommand.Execute(null);
        Assert.True(vm.QrRegenerado);
    }
}
