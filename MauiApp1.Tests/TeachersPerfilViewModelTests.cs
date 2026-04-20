using Xunit;
using MauiApp1.src.Presentation.ViewModels.Teachers;
using System;

namespace MauiApp1.Tests.ViewModels.Teachers;

public class TeachersPerfilViewModelTests
{
    // ── Datos iniciales ───────────────────────────────────

    [Fact]
    public void Constructor_NombreDebeEstarDefinido()
    {
        var vm = new TeachersPerfilViewModel();
        Assert.False(string.IsNullOrEmpty(vm.Nombre));
    }

    [Fact]
    public void Constructor_CorreoDebeEstarDefinido()
    {
        var vm = new TeachersPerfilViewModel();
        Assert.False(string.IsNullOrEmpty(vm.Correo));
    }

    [Fact]
    public void Constructor_ReporteGeneradoDebeIniciarEnFalse()
    {
        var vm = new TeachersPerfilViewModel();
        Assert.False(vm.ReporteGenerado);
    }

    [Fact]
    public void Constructor_MensajeReporteDebeEstarVacioAlInicio()
    {
        var vm = new TeachersPerfilViewModel();
        Assert.Equal(string.Empty, vm.MensajeReporte);
    }

    [Fact]
    public void Constructor_TiempoExpiracionQRDebeSerMayorACero()
    {
        var vm = new TeachersPerfilViewModel();
        Assert.True(vm.TiempoExpiracionQR > 0);
    }

    // ── Reportes ──────────────────────────────────────────

    [Fact]
    public void GenerarReporteDiarioCommand_DebeMarcarReporteComoGenerado()
    {
        var vm = new TeachersPerfilViewModel();
        vm.GenerarReporteDiarioCommand.Execute(null);
        Assert.True(vm.ReporteGenerado);
    }

    [Fact]
    public void GenerarReporteDiarioCommand_MensajeDebeIndicarReporteDiario()
    {
        var vm = new TeachersPerfilViewModel();
        vm.GenerarReporteDiarioCommand.Execute(null);
        Assert.Contains("diario", vm.MensajeReporte.ToLower());
    }

    [Fact]
    public void GenerarReporteSemanalCommand_MensajeDebeIndicarReporteSemanal()
    {
        var vm = new TeachersPerfilViewModel();
        vm.GenerarReporteSemanalCommand.Execute(null);
        Assert.Contains("semanal", vm.MensajeReporte.ToLower());
    }

    [Fact]
    public void GenerarReporteFinalCommand_MensajeDebeIndicarReporteFinal()
    {
        var vm = new TeachersPerfilViewModel();
        vm.GenerarReporteFinalCommand.Execute(null);
        Assert.Contains("final", vm.MensajeReporte.ToLower());
    }

    [Fact]
    public void Reportes_CadaTipoDebeGenerarMensajeDiferente()
    {
        var vm = new TeachersPerfilViewModel();

        vm.GenerarReporteDiarioCommand.Execute(null);
        var mensajeDiario = vm.MensajeReporte;

        vm.GenerarReporteSemanalCommand.Execute(null);
        var mensajeSemanal = vm.MensajeReporte;

        vm.GenerarReporteFinalCommand.Execute(null);
        var mensajeFinal = vm.MensajeReporte;

        Assert.NotEqual(mensajeDiario, mensajeSemanal);
        Assert.NotEqual(mensajeSemanal, mensajeFinal);
        Assert.NotEqual(mensajeDiario, mensajeFinal);
    }

    // ── Configuración QR ──────────────────────────────────

    [Fact]
    public void TiempoExpiracionQR_ValorValidoDebeAsignarse()
    {
        var vm = new TeachersPerfilViewModel();
        vm.TiempoExpiracionQR = 10;
        Assert.Equal(10, vm.TiempoExpiracionQR);
    }

    [Fact]
    public void TiempoExpiracionQR_ValorCeroDebeLanzarExcepcion()
    {
        var vm = new TeachersPerfilViewModel();
        Assert.Throws<ArgumentOutOfRangeException>(() => vm.TiempoExpiracionQR = 0);
    }

    [Fact]
    public void TiempoExpiracionQR_ValorNegativoDebeLanzarExcepcion()
    {
        var vm = new TeachersPerfilViewModel();
        Assert.Throws<ArgumentOutOfRangeException>(() => vm.TiempoExpiracionQR = -5);
    }

    // ── Ajuste administrativo ─────────────────────────────

    [Fact]
    public void Constructor_AjusteEnviadoDebeIniciarEnFalse()
    {
        var vm = new TeachersPerfilViewModel();
        Assert.False(vm.AjusteEnviado);
    }

    [Fact]
    public void EnviarAjusteAdministrativoCommand_DebeMarcarAjusteComoEnviado()
    {
        var vm = new TeachersPerfilViewModel();
        vm.EnviarAjusteAdministrativoCommand.Execute(null);
        Assert.True(vm.AjusteEnviado);
    }
}
