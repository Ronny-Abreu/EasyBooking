using EasyBooking.Domain.Entities;
using EasyBooking.Persistence.Context;
using EasyBooking.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyBooking.Persistence.Repositories
{
    public class PaqueteTuristicoRepository : RepositoryBase<PaqueteTuristico>, IPaqueteTuristicoRepository
    {
        public PaqueteTuristicoRepository(EasyBookingDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<PaqueteTuristico>> BuscarPaquetesAsync(string? destino = null, decimal? precioMinimo = null, decimal? precioMaximo = null, int? calificacion = null, int? duracionMinima = null, int? duracionMaxima = null)
        {
            var query = _dbContext.PaquetesTuristicos.Where(p => p.Activo);

            if (!string.IsNullOrEmpty(destino))
            {
                query = query.Where(p => p.Destino.Contains(destino) || p.Pais.Contains(destino));
            }

            if (precioMinimo.HasValue)
            {
                query = query.Where(p => p.Precio >= precioMinimo.Value);
            }

            if (precioMaximo.HasValue)
            {
                query = query.Where(p => p.Precio <= precioMaximo.Value);
            }

            if (calificacion.HasValue)
            {
                query = query.Where(p => p.Calificacion >= calificacion.Value);
            }

            if (duracionMinima.HasValue)
            {
                query = query.Where(p => p.Duracion >= duracionMinima.Value);
            }

            if (duracionMaxima.HasValue)
            {
                query = query.Where(p => p.Duracion <= duracionMaxima.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<PaqueteTuristico?> GetPaqueteWithImagesAsync(int id)
        {
            return await _dbContext.PaquetesTuristicos
                .Include(p => p.Imagenes.Where(i => i.Activo).OrderBy(i => i.Orden))
                .FirstOrDefaultAsync(p => p.Id == id && p.Activo);
        }
    }
}
