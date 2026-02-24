using Microsoft.Maui.Graphics;

namespace MauiApp1.src.Core.Models
{
    public class ClaseInfo
    {
        public string Nombre { get; set; }
        public string Profesor { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string Salon { get; set; }
        public string Tipo { get; set; }
        public int Porcentaje { get; set; }
        public Color Acento { get; set; }

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