namespace EasyBooking.Application.Dtos
{
    public class PagoDto
    {
        public int ReservaId { get; set; }
        public string NumeroTarjeta { get; set; } = string.Empty;
        public string NombreTitular { get; set; } = string.Empty;
        public string FechaExpiracion { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
    }
}