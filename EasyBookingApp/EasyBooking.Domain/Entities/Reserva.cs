using EasyBooking.Domain.Core;

namespace EasyBooking.Domain.Entities
{
    public class Reserva : ClaseBase
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int NumeroHuespedes { get; set; }
        public decimal PrecioTotal { get; set; }
        public EstadoReserva Estado { get; set; } = EstadoReserva.Pendiente;
        public string? ReferenciaPago { get; set; }
    }

    public enum EstadoReserva
    {
        Pendiente,
        Confirmada,
        Cancelada,
        Completada
    }
}