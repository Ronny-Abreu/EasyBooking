using EasyBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyBooking.Persistence.Context
{
    public class EasyBookingDbContext : DbContext
    {
        public EasyBookingDbContext(DbContextOptions<EasyBookingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Publicacion> Publicaciones { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Valoracion> Valoraciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de claves primarias (aunque en este caso Entity Framework debería inferirlo automáticamente)
            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);
            modelBuilder.Entity<Publicacion>().HasKey(p => p.Id);
            modelBuilder.Entity<Reserva>().HasKey(r => r.Id);
            modelBuilder.Entity<Valoracion>().HasKey(v => v.Id);

            // Configuraciones de relaciones
            modelBuilder.Entity<Publicacion>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Publicaciones)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict); // Añadir restricción para evitar eliminación en cascada

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reservas)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina un usuario, eliminar las reservas

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Publicacion)
                .WithMany(p => p.Reservas)
                .HasForeignKey(r => r.PublicacionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Valoracion>()
                .HasOne(v => v.Usuario)
                .WithMany(u => u.Valoraciones)
                .HasForeignKey(v => v.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Valoracion>()
                .HasOne(v => v.Publicacion)
                .WithMany(p => p.Valoraciones)
                .HasForeignKey(v => v.PublicacionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de propiedades opcionales
            modelBuilder.Entity<Publicacion>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");  // 18 es la precisión y 2 es la escala (decimales)

            modelBuilder.Entity<Reserva>()
                .Property(r => r.PrecioTotal)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Reserva>()
                .Property(r => r.Total)
                .HasColumnType("decimal(18,2)");


            // Configuración de longitud máxima de cadenas
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Telefono)
                .HasMaxLength(20);

            modelBuilder.Entity<Publicacion>()
                .Property(p => p.ImagenUrl)
                .HasMaxLength(500);

            // Definición de índices si es necesario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Publicacion>()
                .HasIndex(p => p.Titulo);

            // Crear datos iniciales después de la creación del modelo
            modelBuilder.Entity<Usuario>().HasData(
            new Usuario
                {
                    Id = 20,
                    Nombre = "Admin2",
                    Apellido = "Admin",
                    Email = "admin@easybooking.com",
                    Username = "admin",
                    Password = "hashedpassword",
                    Telefono = "1234567890",
                    ResetCode = null,  // Add this
                    ResetCodeExpiry = null  // Add this
            }
            );

        }
    }
}