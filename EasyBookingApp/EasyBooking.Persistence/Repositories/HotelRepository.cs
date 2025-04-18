using EasyBooking.Domain.Entities;
using EasyBooking.Persistence.Context;
using EasyBooking.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyBooking.Persistence.Repositories
{
    public class HotelRepository : RepositoryBase<Hotel>, IHotelRepository
    {
        public HotelRepository(EasyBookingDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Hotel>> BuscarHotelesAsync(string? ciudad = null, decimal? precioMinimo = null, decimal? precioMaximo = null, int? calificacion = null)
        {
            var query = _dbContext.Hoteles.Where(h => h.Activo);

            if (!string.IsNullOrEmpty(ciudad))
            {
                query = query.Where(h => h.Ciudad.Contains(ciudad));
            }

            if (precioMinimo.HasValue)
            {
                query = query.Where(h => h.PrecioPorNoche >= precioMinimo.Value);
            }

            if (precioMaximo.HasValue)
            {
                query = query.Where(h => h.PrecioPorNoche <= precioMaximo.Value);
            }

            if (calificacion.HasValue)
            {
                query = query.Where(h => h.Calificacion >= calificacion.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Hotel?> GetHotelWithImagesAsync(int id)
        {
            return await _dbContext.Hoteles
                .Include(h => h.Imagenes.Where(i => i.Activo).OrderBy(i => i.Orden))
                .FirstOrDefaultAsync(h => h.Id == id && h.Activo);
        }
    }
}