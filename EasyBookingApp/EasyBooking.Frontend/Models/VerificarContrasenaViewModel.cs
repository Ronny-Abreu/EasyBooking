using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Frontend.Models
{
    public class VerificarContrasenaViewModel
    {
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = string.Empty;
    }
}