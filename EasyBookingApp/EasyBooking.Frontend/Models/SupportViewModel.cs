using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Frontend.Models
{
    public class SupportViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Por favor, introduce un correo electrónico válido")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La consulta es obligatoria")]
        [Display(Name = "¿Tienes dudas? Desarróllala:")]
        [MinLength(10, ErrorMessage = "Por favor, proporciona más detalles en tu consulta")]
        public string Message { get; set; } = string.Empty;
    }
}
