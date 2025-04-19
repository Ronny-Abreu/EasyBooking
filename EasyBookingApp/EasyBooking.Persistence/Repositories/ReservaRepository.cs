using EasyBooking.Domain.Entities;
using EasyBooking.Persistence.Context;
using EasyBooking.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyBooking.Persistence.Repositories
{
    public class ReservaRepository : RepositoryBase<Reserva>, IReservaRepository
    {
        public ReservaRepository(EasyBookingDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Reserva>> GetReservasByUsuarioIdAsync(int usuarioId)
        {
            return await _dbContext.Reservas
                .Include(r => r.Hotel)
                .Where(r => r.UsuarioId == usuarioId && r.Activo)
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }

        public async Task<bool> ExisteReservaEnFechasAsync(int hotelId, DateTime fechaEntrada, DateTime fechaSalida)
        {
            return await _dbContext.Reservas
                .AnyAsync(r => r.HotelId == hotelId && r.Activo && r.Estado != EstadoReserva.Cancelada &&
                              ((fechaEntrada >= r.FechaEntrada && fechaEntrada < r.FechaSalida) ||
                               (fechaSalida > r.FechaEntrada && fechaSalida <= r.FechaSalida) ||
                               (fechaEntrada <= r.FechaEntrada && fechaSalida >= r.FechaSalida)));
        }

        public async Task<Reserva?> GetReservaByIdWithDetailsAsync(int id)
        {
            return await _dbContext.Reservas
                .Include(r => r.Hotel)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id && r.Activo);
        }
    }
}