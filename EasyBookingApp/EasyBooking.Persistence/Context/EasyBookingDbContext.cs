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
        public DbSet<HotelImagen> HotelImagenes { get; set; } // Nueva entidad

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de HotelImagen
            modelBuilder.Entity<HotelImagen>()
                .HasOne(hi => hi.Hotel)
                .WithMany(h => h.Imagenes)
                .HasForeignKey(hi => hi.HotelId)
                .OnDelete(DeleteBehavior.Cascade);


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

            // Datos semilla para imágenes de hoteles
            modelBuilder.Entity<HotelImagen>().HasData(
                // Hotel 1 - Paraíso
                new HotelImagen
                {
                    Id = 1,
                    HotelId = 1,
                    Url = "/img/hotels/hotel1.jpg",
                    Titulo = "Vista principal",
                    EsPrincipal = true,
                    Orden = 1,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },
                new HotelImagen
                {
                    Id = 2,
                    HotelId = 1,
                    Url = "/img/hotels/hotel1_2.jpg",
                    Titulo = "Habitación",
                    EsPrincipal = false,
                    Orden = 2,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },
                new HotelImagen
                {
                    Id = 3,
                    HotelId = 1,
                    Url = "/img/hotels/hotel1_3.jpg",
                    Titulo = "Piscina",
                    EsPrincipal = false,
                    Orden = 3,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },

                // Hotel 2 - Montaña
                new HotelImagen
                {
                    Id = 4,
                    HotelId = 2,
                    Url = "/img/hotels/hotel2.jpg",
                    Titulo = "Vista principal",
                    EsPrincipal = true,
                    Orden = 1,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },
                new HotelImagen
                {
                    Id = 5,
                    HotelId = 2,
                    Url = "/img/hotels/hotel2_2.jpg",
                    Titulo = "Habitación",
                    EsPrincipal = false,
                    Orden = 2,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },

                // Hotel 3 - Céntrico
                new HotelImagen
                {
                    Id = 6,
                    HotelId = 3,
                    Url = "/img/hotels/hotel3.jpg",
                    Titulo = "Vista principal",
                    EsPrincipal = true,
                    Orden = 1,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },
                new HotelImagen
                {
                    Id = 7,
                    HotelId = 3,
                    Url = "/img/hotels/hotel3_2.jpg",
                    Titulo = "Restaurante",
                    EsPrincipal = false,
                    Orden = 2,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                }
            );
        }
    }
}