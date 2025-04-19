﻿// <auto-generated />
using System;
using EasyBooking.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EasyBooking.Persistence.Migrations
{
    [DbContext(typeof(EasyBookingDbContext))]
    [Migration("20250418132503_actualizacionEntidadHoteles")]
    partial class actualizacionEntidadHoteles
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EasyBooking.Domain.Entities.Reserva", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaEntrada")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaSalida")
                        .HasColumnType("datetime2");

                    b.Property<int>("HotelId")
                        .HasColumnType("int");

                    b.Property<int>("NumeroHuespedes")
                        .HasColumnType("int");

                    b.Property<decimal>("PrecioTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ReferenciaPago")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Reservas");
                });

            modelBuilder.Entity("EasyBooking.Domain.Entities.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Hotel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<int>("Calificacion")
                        .HasColumnType("int");

                    b.Property<string>("Ciudad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagenUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Latitud")
                        .HasColumnType("float");

                    b.Property<double?>("Longitud")
                        .HasColumnType("float");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pais")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PrecioPorNoche")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Servicios")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Hoteles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Activo = true,
                            Calificacion = 5,
                            Ciudad = "Cancún",
                            Descripcion = "Un lujoso hotel con vistas al mar",
                            Direccion = "Calle Principal 123",
                            FechaCreacion = new DateTime(2025, 4, 18, 9, 25, 2, 611, DateTimeKind.Local).AddTicks(5823),
                            ImagenUrl = "/img/hotels/hotel1.jpg",
                            Nombre = "Hotel Paraíso",
                            Pais = "México",
                            PrecioPorNoche = 150.00m,
                            Servicios = ""
                        },
                        new
                        {
                            Id = 2,
                            Activo = true,
                            Calificacion = 4,
                            Ciudad = "Bariloche",
                            Descripcion = "Disfruta de la naturaleza en nuestro hotel de montaña",
                            Direccion = "Avenida Sierra 456",
                            FechaCreacion = new DateTime(2025, 4, 18, 9, 25, 2, 611, DateTimeKind.Local).AddTicks(5836),
                            ImagenUrl = "/img/hotels/hotel2.jpg",
                            Nombre = "Hotel Montaña",
                            Pais = "Argentina",
                            PrecioPorNoche = 120.00m,
                            Servicios = ""
                        },
                        new
                        {
                            Id = 3,
                            Activo = true,
                            Calificacion = 4,
                            Ciudad = "Madrid",
                            Descripcion = "Ubicado en el corazón de la ciudad",
                            Direccion = "Plaza Mayor 789",
                            FechaCreacion = new DateTime(2025, 4, 18, 9, 25, 2, 611, DateTimeKind.Local).AddTicks(5838),
                            ImagenUrl = "/img/hotels/hotel3.jpg",
                            Nombre = "Hotel Céntrico",
                            Pais = "España",
                            PrecioPorNoche = 180.00m,
                            Servicios = ""
                        });
                });

            modelBuilder.Entity("EasyBooking.Domain.Entities.Reserva", b =>
                {
                    b.HasOne("Hotel", "Hotel")
                        .WithMany("Reservas")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EasyBooking.Domain.Entities.Usuario", "Usuario")
                        .WithMany("Reservas")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("EasyBooking.Domain.Entities.Usuario", b =>
                {
                    b.Navigation("Reservas");
                });

            modelBuilder.Entity("Hotel", b =>
                {
                    b.Navigation("Reservas");
                });
#pragma warning restore 612, 618
        }
    }
}
