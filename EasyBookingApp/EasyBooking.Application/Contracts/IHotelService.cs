using EasyBooking.Application.Dtos;

namespace EasyBooking.Application.Contracts
{
    public interface IHotelService
    {
        Task<List<HotelDto>> GetAllHotelesAsync();
        Task<HotelDto?> GetHotelByIdAsync(int id);
        Task<List<HotelDto>> BuscarHotelesAsync(string? ciudad = null, decimal? precioMinimo = null, decimal? precioMaximo = null, int? calificacion = null);
    }
}