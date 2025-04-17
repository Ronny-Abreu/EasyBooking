using EasyBooking.Application.Dtos;

namespace EasyBooking.Application.Contracts
{
    public interface IReservaService
    {
        Task<ReservaDto?> CrearReservaAsync(CrearReservaDto reservaDto, int usuarioId);
        Task<ReservaDto?> GetReservaByIdAsync(int id);
        Task<List<ReservaDto>> GetReservasByUsuarioIdAsync(int usuarioId);
        Task<ReservaDto?> ActualizarReservaAsync(int id, CrearReservaDto reservaDto);
        Task<bool> CancelarReservaAsync(int id);
        Task<ReservaDto?> ProcesarPagoAsync(PagoDto pagoDto);
    }
}