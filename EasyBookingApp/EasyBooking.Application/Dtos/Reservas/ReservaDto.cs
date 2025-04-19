using EasyBooking.Application.Dtos;
using EasyBooking.Domain.Entities;

namespace EasyBooking.Application.Dtos
{
    public class ReservaDto : DtoBase
    {
        public int UsuarioId { get; set; }
        public UsuarioDto? Usuario { get; set; }
        public int HotelId { get; set; }
        public HotelDto? Hotel { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int NumeroHuespedes { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Estado { get; set; } = EstadoReserva.Pendiente.ToString();
        public string? ReferenciaPago { get; set; }
    }
}