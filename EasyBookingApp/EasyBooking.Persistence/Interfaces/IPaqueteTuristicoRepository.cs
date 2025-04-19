using EasyBooking.Domain.Entities;

namespace EasyBooking.Persistence.Interfaces
{
    public interface IPaqueteTuristicoRepository : IRepositoryBase<PaqueteTuristico>
    {
        Task<IReadOnlyList<PaqueteTuristico>> BuscarPaquetesAsync(string? destino = null, decimal? precioMinimo = null, decimal? precioMaximo = null, int? calificacion = null, int? duracionMinima = null, int? duracionMaxima = null);
        Task<PaqueteTuristico?> GetPaqueteWithImagesAsync(int id);
    }
}
