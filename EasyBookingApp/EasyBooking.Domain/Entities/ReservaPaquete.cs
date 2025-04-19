using EasyBooking.Domain.Core;

namespace EasyBooking.Domain.Entities
{
    public class ReservaPaquete : ClaseBase
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public int PaqueteId { get; set; }
        public PaqueteTuristico Paquete { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public int NumeroPersonas { get; set; }
        public decimal PrecioTotal { get; set; }
        public EstadoReservaPaquete Estado { get; set; } = EstadoReservaPaquete.Pendiente;
        public string? ReferenciaPago { get; set; }
    }

    public enum EstadoReservaPaquete
    {
        Pendiente,
        Confirmada,
        Cancelada,
        Completada
    }
}
