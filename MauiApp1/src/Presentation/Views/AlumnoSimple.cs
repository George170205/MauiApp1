using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.src.Presentation.Views
{
    public class AlumnoSimple
    {
        public int AlumnoID { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Matricula { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Telefono { get; set; }
        public bool Activo { get; set; } = true;
        public int UsuarioID { get; set; }
    }
}
