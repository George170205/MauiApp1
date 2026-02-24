using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.src.Core.Models
{
    public class Materia
    {
        public string Nombre { get; set; }
        public string Profesor { get; set; }
        public string Horario { get; set; }
        public string Salon { get; set; }
        public Color Acento { get; set; }
        public int Porcentaje { get; set; }

        public string Iniciales
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Nombre)) return "";

                var palabras = Nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (palabras.Length == 1)
                    return palabras[0][0].ToString().ToUpper();

                return $"{palabras[0][0]}{palabras[1][0]}".ToUpper();
            }
        }
    }
}