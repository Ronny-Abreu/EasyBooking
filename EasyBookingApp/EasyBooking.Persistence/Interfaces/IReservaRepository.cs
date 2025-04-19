using EasyBooking.Domain.Entities;

namespace EasyBooking.Persistence.Interfaces
{
    public interface IReservaRepository : IRepositoryBase<Reserva>
    {
        Task<IReadOnlyList<Reserva>> GetReservasByUsuarioIdAsync(int usuarioId);
        Task<bool> ExisteReservaEnFechasAsync(int hotelId, DateTime fechaEntrada, DateTime fechaSalida);
        Task<Reserva?> GetReservaByIdWithDetailsAsync(int id);
    }
}