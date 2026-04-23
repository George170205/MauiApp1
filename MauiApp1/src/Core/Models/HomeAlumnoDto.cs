namespace MauiApp1.src.Core.Models
{
    public class HomeAlumnoDto
    {
        public string Nombre { get; set; } = "";
        public string Matricula { get; set; } = "";
        public string Carrera { get; set; } = "";
        public double PorcentajeAsistencia { get; set; }
        public int MateriasActivas { get; set; }
        public double Promedio { get; set; }
        public ProximaClaseDto? ProximaClase { get; set; }
    }

    public class ProximaClaseDto
    {
        public string Nombre { get; set; } = "";
        public string Docente { get; set; } = "";
        public string HoraInicio { get; set; } = "";
        public string HoraFin { get; set; } = "";
        public string Aula { get; set; } = "";
    }
}
