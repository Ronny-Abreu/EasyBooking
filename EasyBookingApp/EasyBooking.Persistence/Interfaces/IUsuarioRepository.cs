using EasyBooking.Domain.Entities;

namespace EasyBooking.Persistence.Interfaces
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<bool> ExisteEmailAsync(string email);
        Task DeleteAsync(Usuario usuario);

    }
}