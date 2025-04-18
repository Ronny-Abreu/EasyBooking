using EasyBooking.Domain.Entities;

namespace EasyBooking.Persistence.Interfaces
{
    public interface IHotelRepository : IRepositoryBase<Hotel>
    {
        Task<IReadOnlyList<Hotel>> BuscarHotelesAsync(string? ciudad = null, decimal? precioMinimo = null, decimal? precioMaximo = null, int? calificacion = null);

        // Nuevo método para obtener hotel con imágenes
        Task<Hotel?> GetHotelWithImagesAsync(int id);
    }
}