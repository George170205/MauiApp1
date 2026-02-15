namespace MauiApp1.src.Core.Models
{
    public class RegisterRequest
    {
        public int RolID { get; set; }

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;

        public string? Telefono { get; set; }
    }
}
