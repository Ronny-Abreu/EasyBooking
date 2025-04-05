namespace EasyBooking.Application.Dtos
{
    public class UpdateProfileDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public bool IsEmailVerified { get; set; }
        public string? Password { get; set; }
        public string? CurrentPassword { get; set; }
    }
}
