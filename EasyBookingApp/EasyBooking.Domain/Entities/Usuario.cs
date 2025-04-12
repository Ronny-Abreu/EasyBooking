using EasyBooking.Domain.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Domain.Entities
{
    public class Usuario : ClaseBase
    {
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string Apellido { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [MaxLength(15)]
        public string Telefono { get; set; }

        // Nuevo campo para verificación de correo
        public bool IsEmailVerified { get; set; }

        public string? AvatarUrl { get; set; }

        public string? ResetCode { get; set; }
        public DateTime? ResetCodeExpiry { get; set; }


        // Navegación
        public ICollection<Reserva> Reservas { get; set; }
        public ICollection<Valoracion> Valoraciones { get; set; }

        public Usuario()
        {
            Nombre = string.Empty;
            Apellido = string.Empty;
            Email = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            Telefono = string.Empty;
            Reservas = new List<Reserva>();
            Valoraciones = new List<Valoracion>();
        }
    }
}
