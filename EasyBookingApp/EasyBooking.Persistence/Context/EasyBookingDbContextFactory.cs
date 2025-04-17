using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EasyBooking.Persistence.Context;

namespace EasyBooking.Persistence
{
    public class EasyBookingDbContextFactory : IDesignTimeDbContextFactory<EasyBookingDbContext>
    {
        public EasyBookingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EasyBookingDbContext>();

            // Aquí colocas tu cadena de conexión:
            optionsBuilder.UseSqlServer("Server=SLIMREAPER;Database=EasyBookingDbCrudReserva;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true");

            return new EasyBookingDbContext(optionsBuilder.Options);
        }
    }
}
