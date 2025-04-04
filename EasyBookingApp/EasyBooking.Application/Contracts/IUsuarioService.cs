using EasyBooking.Domain.Entities;
using System.Threading.Tasks;

namespace EasyBooking.Application.Contracts
{
    public interface IUsuarioService
    {
        Task<Usuario> ObtenerUsuarioPorIdAsync(int id);
        Task<IEnumerable<Usuario>> ObtenerUsuariosAsync();
        Task<Usuario> CrearUsuarioAsync(Usuario usuario);
        Task<Usuario> ActualizarUsuarioAsync(Usuario usuario);
        Task EliminarUsuarioAsync(int id);
        Task<Usuario> ObtenerPorEmailAsync(string email);
        Task<Usuario> ObtenerPorUsernameAsync(string username);
    }
}