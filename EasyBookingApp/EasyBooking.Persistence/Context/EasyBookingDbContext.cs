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
        public DbSet<HotelImagen> HotelImagenes { get; set; }
        public DbSet<PaqueteTuristico> PaquetesTuristicos { get; set; }
        public DbSet<PaqueteImagen> PaqueteImagenes { get; set; }
        public DbSet<ReservaPaquete> ReservasPaquetes { get; set; }

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

            // Configuración de PaqueteImagen
            modelBuilder.Entity<PaqueteImagen>()
                .HasOne(pi => pi.Paquete)
                .WithMany(p => p.Imagenes)
                .HasForeignKey(pi => pi.PaqueteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de ReservaPaquete
            modelBuilder.Entity<ReservaPaquete>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.ReservasPaquetes)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReservaPaquete>()
                .HasOne(r => r.Paquete)
                .WithMany(p => p.Reservas)
                .HasForeignKey(r => r.PaqueteId)
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

            // Datos semilla para paquetes turísticos
            modelBuilder.Entity<PaqueteTuristico>().HasData(
                new PaqueteTuristico
                {
                    Id = 1,
                    Nombre = "Aventura en Riviera Maya",
                    Descripcion = "Descubre las maravillas de la Riviera Maya con este paquete que incluye visitas a cenotes, zonas arqueológicas y playas paradisíacas.",
                    Destino = "Riviera Maya",
                    Pais = "México",
                    Duracion = 7,
                    Precio = 1200.00m,
                    Calificacion = 5,
                    ImagenUrl = "/img/packages/package1.jpg",
                    Incluye = "Hospedaje en hotel 5 estrellas, Desayunos, Traslados, Entradas a zonas arqueológicas, Tour a cenotes",
                    NoIncluye = "Vuelos, Comidas y cenas, Gastos personales",
                    Itinerario = "Día 1: Llegada y check-in, Día 2: Visita a Tulum, Día 3: Cenotes, Día 4: Playa del Carmen, Día 5: Xcaret, Día 6: Día libre, Día 7: Check-out y despedida",
                    Latitud = 20.6296,
                    Longitud = -87.0739,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },
                new PaqueteTuristico
                {
                    Id = 2,
                    Nombre = "Maravillas Machu Picchu",
                    Descripcion = "Explora la antigua ciudad inca de Machu Picchu y descubre la rica cultura peruana en este fascinante viaje.",
                    Destino = "Cusco",
                    Pais = "Perú",
                    Duracion = 5,
                    Precio = 950.00m,
                    Calificacion = 4,
                    ImagenUrl = "/img/packages/package2.jpg",
                    Incluye = "Hospedaje, Desayunos, Guía turístico, Entradas a Machu Picchu, Tren a Aguas Calientes",
                    NoIncluye = "Vuelos, Almuerzos y cenas, Propinas",
                    Itinerario = "Día 1: Llegada a Cusco, Día 2: City tour en Cusco, Día 3: Valle Sagrado, Día 4: Machu Picchu, Día 5: Regreso",
                    Latitud = -13.1631,
                    Longitud = -72.5450,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },
                new PaqueteTuristico
                {
                    Id = 3,
                    Nombre = "Escapada a Punta Cana",
                    Descripcion = "Disfruta de las mejores playas del Caribe en este paquete todo incluido en Punta Cana.",
                    Destino = "Punta Cana",
                    Pais = "República Dominicana",
                    Duracion = 4,
                    Precio = 850.00m,
                    Calificacion = 5,
                    ImagenUrl = "/img/packages/package3.jpg",
                    Incluye = "Hospedaje todo incluido, Traslados, Acceso a todas las instalaciones del resort",
                    NoIncluye = "Vuelos, Excursiones adicionales, Servicios de spa",
                    Itinerario = "Día 1: Llegada y check-in, Día 2-3: Disfrute de playa y actividades del resort, Día 4: Check-out",
                    Latitud = 18.5601,
                    Longitud = -68.3725,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                }
            );

            // Datos semilla para imágenes de paquetes
            modelBuilder.Entity<PaqueteImagen>().HasData(
                // Paquete 1 - Riviera Maya
                new PaqueteImagen
                {
                    Id = 1,
                    PaqueteId = 1,
                    Url = "/img/packages/package1.jpg",
                    Titulo = "Vista principal",
                    EsPrincipal = true,
                    Orden = 1,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },
                new PaqueteImagen
                {
                    Id = 2,
                    PaqueteId = 1,
                    Url = "/img/packages/package1_2.jpg",
                    Titulo = "Cenote",
                    EsPrincipal = false,
                    Orden = 2,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },
                new PaqueteImagen
                {
                    Id = 3,
                    PaqueteId = 1,
                    Url = "/img/packages/package1_3.jpg",
                    Titulo = "Playa",
                    EsPrincipal = false,
                    Orden = 3,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },

                // Paquete 2 - Machu Picchu
                new PaqueteImagen
                {
                    Id = 4,
                    PaqueteId = 2,
                    Url = "/img/packages/package2.jpg",
                    Titulo = "Vista principal",
                    EsPrincipal = true,
                    Orden = 1,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },
                new PaqueteImagen
                {
                    Id = 5,
                    PaqueteId = 2,
                    Url = "/img/packages/package2_2.jpg",
                    Titulo = "Cusco",
                    EsPrincipal = false,
                    Orden = 2,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },

                // Paquete 3 - Punta Cana
                new PaqueteImagen
                {
                    Id = 6,
                    PaqueteId = 3,
                    Url = "/img/packages/package3.jpg",
                    Titulo = "Vista principal",
                    EsPrincipal = true,
                    Orden = 1,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                },
                new PaqueteImagen
                {
                    Id = 7,
                    PaqueteId = 3,
                    Url = "/img/packages/package3_2.jpg",
                    Titulo = "Resort",
                    EsPrincipal = false,
                    Orden = 2,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                }
            );
        }
    }
}
