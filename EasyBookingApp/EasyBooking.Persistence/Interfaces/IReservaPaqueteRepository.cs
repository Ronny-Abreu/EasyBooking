using EasyBooking.Domain.Entities;

namespace EasyBooking.Persistence.Interfaces
{
    public interface IReservaPaqueteRepository : IRepositoryBase<ReservaPaquete>
    {
        Task<IReadOnlyList<ReservaPaquete>> GetReservasByUsuarioIdAsync(int usuarioId);
        Task<ReservaPaquete?> GetReservaByIdWithDetailsAsync(int id);
    }
}
