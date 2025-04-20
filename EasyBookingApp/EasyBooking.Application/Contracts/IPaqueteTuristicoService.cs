using EasyBooking.Application.Dtos;

namespace EasyBooking.Application.Contracts
{
    public interface IPaqueteTuristicoService
    {
        Task<List<PaqueteTuristicoDto>> GetAllPaquetesAsync();
        Task<PaqueteTuristicoDto?> GetPaqueteByIdAsync(int id);
        Task<List<PaqueteTuristicoDto>> BuscarPaquetesAsync(string? destino = null, decimal? precioMinimo = null, decimal? precioMaximo = null, int? calificacion = null, int? duracionMinima = null, int? duracionMaxima = null);
    }
}
