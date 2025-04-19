namespace EasyBooking.Application.Dtos
{
    public class CrearReservaDto
    {
        public int HotelId { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int NumeroHuespedes { get; set; }
    }
}