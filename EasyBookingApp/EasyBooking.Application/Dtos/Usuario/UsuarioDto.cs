using EasyBooking.Application.Dtos;

namespace EasyBooking.Application.Dtos
{
    public class UsuarioDto : DtoBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public bool EmailValidado { get; set; }
    }
}