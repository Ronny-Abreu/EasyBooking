using EasyBooking.Application.Dtos;

namespace EasyBooking.Application.Contracts
{
    public interface IUsuarioService
    {
        Task<UsuarioDto?> RegistrarUsuarioAsync(RegistroUsuarioDto registroDto);
        Task<UsuarioDto?> LoginAsync(LoginUsuarioDto loginDto);
        Task<bool> EliminarUsuarioAsync(int id);
        Task<bool> ValidarContrasenaAsync(int id, string password);
        Task<UsuarioDto?> ActualizarUsuarioAsync(ActualizarUsuarioDto dto);
        Task<UsuarioDto?> GetUsuarioByIdAsync(int id);
        Task<UsuarioDto?> GetUsuarioByEmailAsync(string email);
    }
}