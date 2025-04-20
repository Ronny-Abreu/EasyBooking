using EasyBooking.Domain.Entities;
using EasyBooking.Persistence.Context;
using EasyBooking.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyBooking.Persistence.Repositories
{
    public class ReservaPaqueteRepository : RepositoryBase<ReservaPaquete>, IReservaPaqueteRepository
    {
        public ReservaPaqueteRepository(EasyBookingDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<ReservaPaquete>> GetReservasByUsuarioIdAsync(int usuarioId)
        {
            return await _dbContext.ReservasPaquetes
                .Include(r => r.Paquete)
                .Where(r => r.UsuarioId == usuarioId && r.Activo)
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }

        public async Task<ReservaPaquete?> GetReservaByIdWithDetailsAsync(int id)
        {
            return await _dbContext.ReservasPaquetes
                .Include(r => r.Paquete)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id && r.Activo);
        }
    }
}
