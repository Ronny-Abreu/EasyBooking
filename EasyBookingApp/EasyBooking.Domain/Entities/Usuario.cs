using EasyBooking.Domain.Core;

namespace EasyBooking.Domain.Entities
{
    public class Usuario : ClaseBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}