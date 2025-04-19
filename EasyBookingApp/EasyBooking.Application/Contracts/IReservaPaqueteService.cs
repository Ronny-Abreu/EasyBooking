using EasyBooking.Application.Dtos;

namespace EasyBooking.Application.Contracts
{
    public interface IReservaPaqueteService
    {
        Task<ReservaPaqueteDto?> CrearReservaPaqueteAsync(CrearReservaPaqueteDto reservaDto, int usuarioId);
        Task<ReservaPaqueteDto?> GetReservaPaqueteByIdAsync(int id);
        Task<List<ReservaPaqueteDto>> GetReservasPaqueteByUsuarioIdAsync(int usuarioId);
        Task<bool> CancelarReservaPaqueteAsync(int id);
        Task<ReservaPaqueteDto?> ProcesarPagoAsync(PagoDto pagoDto);
    }
}
