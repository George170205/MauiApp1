using Xunit;
using MauiApp1.src.Presentation.ViewModels.Teachers;
using System.Linq;
using System;

namespace MauiApp1.Tests.ViewModels.Teachers;

public class TeachersHorarioViewModelTests
{
    // ── Inicialización ────────────────────────────────────

    [Fact]
    public void Constructor_DebeCrear5DiasDeSemana()
    {
        var vm = new TeachersHorarioViewModel();
        Assert.Equal(5, vm.DiasSemana.Count);
    }

    [Fact]
    public void Constructor_LunesDebeEstarSeleccionadoPorDefecto()
    {
        var vm = new TeachersHorarioViewModel();
        Assert.True(vm.DiasSemana[0].IsSelected);
    }

    [Fact]
    public void Constructor_SolounDiaDebeEstarSeleccionado()
    {
        var vm = new TeachersHorarioViewModel();
        var seleccionados = vm.DiasSemana.Where(d => d.IsSelected).ToList();
        Assert.Single(seleccionados);
    }

    [Fact]
    public void Constructor_DiaActualTextoDebeSerLunes()
    {
        var vm = new TeachersHorarioViewModel();
        Assert.Equal("Lunes", vm.DiaActualTexto);
    }

    [Fact]
    public void Constructor_ClasesDelDiaNoDebeEstarVacio()
    {
        var vm = new TeachersHorarioViewModel();
        Assert.NotEmpty(vm.ClasesDelDia);
    }

    // ── Selección de día ──────────────────────────────────

    [Fact]
    public void SeleccionarDia_DebeActualizarDiaActualTexto()
    {
        var vm = new TeachersHorarioViewModel();
        vm.SeleccionarDiaCommand.Execute(vm.DiasSemana[1]); // Martes
        Assert.Equal("Martes", vm.DiaActualTexto);
    }

    [Fact]
    public void SeleccionarDia_SoloUnDiaDebeQuedarSeleccionado()
    {
        var vm = new TeachersHorarioViewModel();
        vm.SeleccionarDiaCommand.Execute(vm.DiasSemana[1]); // Martes
        var seleccionados = vm.DiasSemana.Where(d => d.IsSelected).ToList();
        Assert.Single(seleccionados);
    }

    [Fact]
    public void SeleccionarDia_ElDiaAnteriorDebeDeseleccionarse()
    {
        var vm = new TeachersHorarioViewModel();
        vm.SeleccionarDiaCommand.Execute(vm.DiasSemana[1]); // Martes
        Assert.False(vm.DiasSemana[0].IsSelected); // Lunes ya no seleccionado
    }

    // ── Carga de clases ───────────────────────────────────

    [Fact]
    public void SeleccionarLunes_DebeCargar2Clases()
    {
        var vm = new TeachersHorarioViewModel();
        vm.SeleccionarDiaCommand.Execute(vm.DiasSemana[0]); // Lunes
        Assert.Equal(2, vm.ClasesDelDia.Count);
    }

    [Fact]
    public void SeleccionarMiercoles_DebeCargar2Clases()
    {
        var vm = new TeachersHorarioViewModel();
        vm.SeleccionarDiaCommand.Execute(vm.DiasSemana[2]); // Miércoles
        Assert.Equal(2, vm.ClasesDelDia.Count);
    }

    [Fact]
    public void SeleccionarMartes_DebeCargar1Clase()
    {
        var vm = new TeachersHorarioViewModel();
        vm.SeleccionarDiaCommand.Execute(vm.DiasSemana[1]); // Martes
        Assert.Single(vm.ClasesDelDia);
    }

    [Fact]
    public void SeleccionarViernes_DebeCargarClaseSinClases()
    {
        var vm = new TeachersHorarioViewModel();
        vm.SeleccionarDiaCommand.Execute(vm.DiasSemana[4]); // Viernes
        Assert.Single(vm.ClasesDelDia);
        Assert.Equal("Sin clases", vm.ClasesDelDia[0].Nombre);
    }
}
