using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EasyBooking.Persistence.Context
{
    public class EasyBookingDbContextFactory : IDesignTimeDbContextFactory<EasyBookingDbContext>
    {
        public EasyBookingDbContext CreateDbContext(string[] args)
        {
            // Usar el mismo archivo de configuración que el de tu API
            var optionsBuilder = new DbContextOptionsBuilder<EasyBookingDbContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).FullName) // Ruta de la carpeta superior a Persistence
                .AddJsonFile("EasyBooking.Api/appsettings.json") // Ruta correcta al archivo
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new EasyBookingDbContext(optionsBuilder.Options);
        }
    }
}