namespace EasyBooking.Application.Dtos
{
    public class UsuarioDto : DtoBase
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Telefono { get; set; }
        public string? Password { get; set; }
        public bool IsEmailVerified { get; set; }
        public string? AvatarUrl { get; set; }

    }
}
