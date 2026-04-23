namespace MauiApp1.src.Core.Models
{
    public class ValidarQrResponse
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = "";
        // El backend puede usar "message" o "mensaje" — PropertyNameCaseInsensitive lo resuelve
        public string Message { get; set; } = "";

        public string MensajeEfectivo => !string.IsNullOrEmpty(Mensaje) ? Mensaje : Message;
    }
}
