using EasyBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyBooking.Persistence.Context
{
    public class EasyBookingDbContext : DbContext
    {
        public EasyBookingDbContext(DbContextOptions<EasyBookingDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Hotel> Hoteles { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configuración de Reserva
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reservas)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Reservas)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Datos semilla para hoteles
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Nombre = "Hotel Paraíso",
                    Descripcion = "Un lujoso hotel con vistas al mar",
                    Direccion = "Calle Principal 123",
                    Ciudad = "Cancún",
                    Pais = "México",
                    ImagenUrl = "/img/hotels/hotel1.jpg",
                    PrecioPorNoche = 150.00m,
                    Calificacion = 5,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },
                new Hotel
                {
                    Id = 2,
                    Nombre = "Hotel Montaña",
                    Descripcion = "Disfruta de la naturaleza en nuestro hotel de montaña",
                    Direccion = "Avenida Sierra 456",
                    Ciudad = "Bariloche",
                    Pais = "Argentina",
                    ImagenUrl = "/img/hotels/hotel2.jpg",
                    PrecioPorNoche = 120.00m,
                    Calificacion = 4,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },
                new Hotel
                {
                    Id = 3,
                    Nombre = "Hotel Céntrico",
                    Descripcion = "Ubicado en el corazón de la ciudad",
                    Direccion = "Plaza Mayor 789",
                    Ciudad = "Madrid",
                    Pais = "España",
                    ImagenUrl = "/img/hotels/hotel3.jpg",
                    PrecioPorNoche = 180.00m,
                    Calificacion = 4,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                }
            );
        }
    }
}