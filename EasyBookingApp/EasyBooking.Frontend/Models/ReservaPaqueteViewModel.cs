using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Frontend.Models
{
    public class ReservaPaqueteViewModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioViewModel? Usuario { get; set; }
        public int PaqueteId { get; set; }
        public PaqueteTuristicoViewModel? Paquete { get; set; }
        public DateTime FechaInicio { get; set; }
        public int NumeroPersonas { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? ReferenciaPago { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CrearReservaPaqueteViewModel
    {
        public int PaqueteId { get; set; }
        public PaqueteTuristicoViewModel? Paquete { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "El número de personas es requerido")]
        [Range(1, 20, ErrorMessage = "El número de personas debe estar entre 1 y 20")]
        [Display(Name = "Número de personas")]
        public int NumeroPersonas { get; set; }
    }

    public class PagoPaqueteViewModel
    {
        public int ReservaId { get; set; }
        public ReservaPaqueteViewModel? Reserva { get; set; }

        [Required(ErrorMessage = "El número de tarjeta es requerido")]
        [CreditCard(ErrorMessage = "El número de tarjeta no es válido")]
        [Display(Name = "Número de tarjeta")]
        public string NumeroTarjeta { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del titular es requerido")]
        [Display(Name = "Nombre del titular")]
        public string NombreTitular { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de expiración es requerida")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{2})$", ErrorMessage = "La fecha de expiración debe tener el formato MM/YY")]
        [Display(Name = "Fecha de expiración (MM/YY)")]
        public string FechaExpiracion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El CVV es requerido")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "El CVV debe tener 3 o 4 dígitos")]
        [Display(Name = "CVV")]
        public string CVV { get; set; } = string.Empty;
    }
}
