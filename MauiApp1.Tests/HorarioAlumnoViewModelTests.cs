using MauiApp1.src.Presentation.ViewModels.Student;
using System.Linq;
using Xunit;

namespace MauiApp1.Tests.ViewModels;

public class HorarioAlumnoViewModelTests
{
    // --- Inicialización ---

    [Fact]
    public void Constructor_DebeCrear5DiasDeSemana()
    {
        var vm = new HorarioAlumnoViewModel();
        Assert.Equal(5, vm.DiasSemana.Count);
    }

    [Fact]
    public void Constructor_MiercolesDebeEstarSeleccionadoPorDefecto()
    {
        var vm = new HorarioAlumnoViewModel();
        var miercoles = vm.DiasSemana[2];
        Assert.True(miercoles.IsSelected);
    }

    [Fact]
    public void Constructor_SoloDiaSeleccionadoDebeSerMiercoles()
    {
        var vm = new HorarioAlumnoViewModel();
        var seleccionados = vm.DiasSemana.Where(d => d.IsSelected).ToList();
        Assert.Single(seleccionados);
        Assert.Equal("Miércoles", seleccionados[0].NombreCompleto);
    }

    [Fact]
    public void Constructor_DiaActualTextoDebeSerMiercoles()
    {
        var vm = new HorarioAlumnoViewModel();
        Assert.Equal("Miércoles", vm.DiaActualTexto);
    }

    [Fact]
    public void Constructor_ClasesDelDiaNoDebeEstarVacio()
    {
        var vm = new HorarioAlumnoViewModel();
        Assert.NotEmpty(vm.ClasesDelDia);
    }

    // --- SeleccionarDia ---

    [Fact]
    public void SeleccionarDia_DebeActualizarDiaActualTexto()
    {
        var vm = new HorarioAlumnoViewModel();
        var lunes = vm.DiasSemana[0];

        vm.SeleccionarDiaCommand.Execute(lunes);

        Assert.Equal("Lunes", vm.DiaActualTexto);
    }

    [Fact]
    public void SeleccionarDia_SoloUnDiaDebeQuedarSeleccionado()
    {
        var vm = new HorarioAlumnoViewModel();

        vm.SeleccionarDiaCommand.Execute(vm.DiasSemana[0]); // Lunes

        var seleccionados = vm.DiasSemana.Where(d => d.IsSelected).ToList();
        Assert.Single(seleccionados);
    }

    [Fact]
    public void SeleccionarDia_ElDiaAnteriorDebeDeseleccionarse()
    {
        var vm = new HorarioAlumnoViewModel();

        vm.SeleccionarDiaCommand.Execute(vm.DiasSemana[0]); // Lunes

        Assert.False(vm.DiasSemana[2].IsSelected); // Miércoles ya no seleccionado
    }

    // --- CargarClases ---

    [Fact]
    public void SeleccionarMiercoles_DebeCargar6Clases()
    {
        var vm = new HorarioAlumnoViewModel();

        vm.SeleccionarDiaCommand.Execute(vm.DiasSemana[2]); // Miércoles

        Assert.Equal(6, vm.ClasesDelDia.Count);
    }

    [Fact]
    public void SeleccionarDiaNoCargado_DebeCargar1ClaseDefault()
    {
        var vm = new HorarioAlumnoViewModel();

        vm.SeleccionarDiaCommand.Execute(vm.DiasSemana[0]); // Lunes

        Assert.Single(vm.ClasesDelDia);
        Assert.Equal("Día Libre o Diferente", vm.ClasesDelDia[0].Nombre);
    }
}