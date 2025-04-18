using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Frontend.Models
{
    public class ReservaViewModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioViewModel? Usuario { get; set; }
        public int HotelId { get; set; }
        public HotelViewModel? Hotel { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int NumeroHuespedes { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? ReferenciaPago { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CrearReservaViewModel
    {
        public int HotelId { get; set; }
        public HotelViewModel? Hotel { get; set; }

        [Required(ErrorMessage = "La fecha de entrada es requerida")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de entrada")]
        public DateTime FechaEntrada { get; set; }

        [Required(ErrorMessage = "La fecha de salida es requerida")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de salida")]
        public DateTime FechaSalida { get; set; }

        [Required(ErrorMessage = "El número de huéspedes es requerido")]
        [Range(1, 10, ErrorMessage = "El número de huéspedes debe estar entre 1 y 10")]
        [Display(Name = "Número de huéspedes")]
        public int NumeroHuespedes { get; set; }
    }

    public class PagoViewModel
    {
        public int ReservaId { get; set; }
        public ReservaViewModel? Reserva { get; set; }

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