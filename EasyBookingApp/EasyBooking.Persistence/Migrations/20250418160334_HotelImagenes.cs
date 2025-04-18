using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EasyBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class HotelImagenes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotelImagenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_HotelImagenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelImagenes_Hoteles_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hoteles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "HotelImagenes",
                columns: new[] { "Id", "Activo", "EsPrincipal", "FechaCreacion", "FechaModificacion", "HotelId", "Orden", "Titulo", "Url" },
                values: new object[,]
                {
                    { 1, true, true, new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8316), null, 1, 1, "Vista principal", "/img/hotels/hotel1.jpg" },
                    { 2, true, false, new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8318), null, 1, 2, "Habitación", "/img/hotels/hotel1_2.jpg" },
                    { 3, true, false, new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8320), null, 1, 3, "Piscina", "/img/hotels/hotel1_3.jpg" },
                    { 4, true, true, new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8322), null, 2, 1, "Vista principal", "/img/hotels/hotel2.jpg" },
                    { 5, true, false, new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8324), null, 2, 2, "Habitación", "/img/hotels/hotel2_2.jpg" },
                    { 6, true, true, new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8325), null, 3, 1, "Vista principal", "/img/hotels/hotel3.jpg" },
                    { 7, true, false, new DateTime(2025, 4, 18, 12, 3, 34, 262, DateTimeKind.Local).AddTicks(8326), null, 3, 2, "Restaurante", "/img/hotels/hotel3_2.jpg" }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_HotelImagenes_HotelId",
                table: "HotelImagenes",
                column: "HotelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelImagenes");

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 9, 25, 2, 611, DateTimeKind.Local).AddTicks(5823));

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 9, 25, 2, 611, DateTimeKind.Local).AddTicks(5836));

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 9, 25, 2, 611, DateTimeKind.Local).AddTicks(5838));
        }
    }
}
