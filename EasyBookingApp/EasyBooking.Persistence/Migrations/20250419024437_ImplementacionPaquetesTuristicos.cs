using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EasyBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ImplementacionPaquetesTuristicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaquetesTuristicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duracion = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Calificacion = table.Column<int>(type: "int", nullable: false),
                    ImagenUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Incluye = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoIncluye = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Itinerario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitud = table.Column<double>(type: "float", nullable: true),
                    Longitud = table.Column<double>(type: "float", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaquetesTuristicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaqueteImagenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaqueteId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EsPrincipal = table.Column<bool>(type: "bit", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaqueteImagenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaqueteImagenes_PaquetesTuristicos_PaqueteId",
                        column: x => x.PaqueteId,
                        principalTable: "PaquetesTuristicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservasPaquetes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    PaqueteId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroPersonas = table.Column<int>(type: "int", nullable: false),
                    PrecioTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    ReferenciaPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservasPaquetes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservasPaquetes_PaquetesTuristicos_PaqueteId",
                        column: x => x.PaqueteId,
                        principalTable: "PaquetesTuristicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservasPaquetes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5459));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5461));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5463));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5464));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5466));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5467));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5469));

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5307));

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5321));

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5324));

            migrationBuilder.InsertData(
                table: "PaquetesTuristicos",
                columns: new[] { "Id", "Activo", "Calificacion", "Descripcion", "Destino", "Duracion", "FechaCreacion", "FechaModificacion", "ImagenUrl", "Incluye", "Itinerario", "Latitud", "Longitud", "NoIncluye", "Nombre", "Pais", "Precio" },
                values: new object[,]
                {
                    { 1, true, 5, "Descubre las maravillas de la Riviera Maya con este paquete que incluye visitas a cenotes, zonas arqueológicas y playas paradisíacas.", "Riviera Maya", 7, new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5502), null, "/img/packages/package1.jpg", "Hospedaje en hotel 5 estrellas, Desayunos, Traslados, Entradas a zonas arqueológicas, Tour a cenotes", "Día 1: Llegada y check-in, Día 2: Visita a Tulum, Día 3: Cenotes, Día 4: Playa del Carmen, Día 5: Xcaret, Día 6: Día libre, Día 7: Check-out y despedida", 20.6296, -87.073899999999995, "Vuelos, Comidas y cenas, Gastos personales", "Aventura en Riviera Maya", "México", 1200.00m },
                    { 2, true, 4, "Explora la antigua ciudad inca de Machu Picchu y descubre la rica cultura peruana en este fascinante viaje.", "Cusco", 5, new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5506), null, "/img/packages/package2.jpg", "Hospedaje, Desayunos, Guía turístico, Entradas a Machu Picchu, Tren a Aguas Calientes", "Día 1: Llegada a Cusco, Día 2: City tour en Cusco, Día 3: Valle Sagrado, Día 4: Machu Picchu, Día 5: Regreso", -13.1631, -72.545000000000002, "Vuelos, Almuerzos y cenas, Propinas", "Maravillas de Machu Picchu", "Perú", 950.00m },
                    { 3, true, 5, "Disfruta de las mejores playas del Caribe en este paquete todo incluido en Punta Cana.", "Punta Cana", 4, new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5509), null, "/img/packages/package3.jpg", "Hospedaje todo incluido, Traslados, Acceso a todas las instalaciones del resort", "Día 1: Llegada y check-in, Día 2-3: Disfrute de playa y actividades del resort, Día 4: Check-out", 18.560099999999998, -68.372500000000002, "Vuelos, Excursiones adicionales, Servicios de spa", "Escapada a Punta Cana", "República Dominicana", 850.00m }
                });

            migrationBuilder.InsertData(
                table: "PaqueteImagenes",
                columns: new[] { "Id", "Activo", "EsPrincipal", "FechaCreacion", "FechaModificacion", "Orden", "PaqueteId", "Titulo", "Url" },
                values: new object[,]
                {
                    { 1, true, true, new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5528), null, 1, 1, "Vista principal", "/img/packages/package1.jpg" },
                    { 2, true, false, new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5530), null, 2, 1, "Cenote", "/img/packages/package1_2.jpg" },
                    { 3, true, false, new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5531), null, 3, 1, "Playa", "/img/packages/package1_3.jpg" },
                    { 4, true, true, new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5533), null, 1, 2, "Vista principal", "/img/packages/package2.jpg" },
                    { 5, true, false, new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5539), null, 2, 2, "Cusco", "/img/packages/package2_2.jpg" },
                    { 6, true, true, new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5541), null, 1, 3, "Vista principal", "/img/packages/package3.jpg" },
                    { 7, true, false, new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5542), null, 2, 3, "Resort", "/img/packages/package3_2.jpg" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaqueteImagenes_PaqueteId",
                table: "PaqueteImagenes",
                column: "PaqueteId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservasPaquetes_PaqueteId",
                table: "ReservasPaquetes",
                column: "PaqueteId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservasPaquetes_UsuarioId",
                table: "ReservasPaquetes",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaqueteImagenes");

            migrationBuilder.DropTable(
                name: "ReservasPaquetes");

            migrationBuilder.DropTable(
                name: "PaquetesTuristicos");

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8316));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8318));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8320));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8322));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8324));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8325));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8326));

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8154));

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8169));

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8173));
        }
    }
}
