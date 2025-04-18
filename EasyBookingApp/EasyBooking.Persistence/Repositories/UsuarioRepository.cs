using EasyBooking.Domain.Entities;
using EasyBooking.Persistence.Context;
using EasyBooking.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyBooking.Persistence.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(EasyBookingDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _dbContext.Usuarios
                .Where(u => u.Email == email && u.Activo)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _dbContext.Usuarios
                .AnyAsync(u => u.Email == email && u.Activo);
        }

        public async Task DeleteAsync(Usuario usuario)
        {
            _dbContext.Usuarios.Remove(usuario);
            await _dbContext.SaveChangesAsync();
        }
    }
}