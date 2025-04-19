using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionEntidadHoteles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                table: "Hoteles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitud",
                table: "Hoteles",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "ImagenUrl", "Latitud", "Longitud" },
                values: new object[] { new DateTime(2025, 4, 18, 9, 25, 2, 611, DateTimeKind.Local).AddTicks(5823), "/img/hotels/hotel1.jpg", null, null });

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "ImagenUrl", "Latitud", "Longitud" },
                values: new object[] { new DateTime(2025, 4, 18, 9, 25, 2, 611, DateTimeKind.Local).AddTicks(5836), "/img/hotels/hotel2.jpg", null, null });

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaCreacion", "ImagenUrl", "Latitud", "Longitud" },
                values: new object[] { new DateTime(2025, 4, 18, 9, 25, 2, 611, DateTimeKind.Local).AddTicks(5838), "/img/hotels/hotel3.jpg", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Hoteles");


            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "ImagenUrl" },
                values: new object[] { new DateTime(2025, 4, 15, 15, 44, 27, 234, DateTimeKind.Local).AddTicks(5190), "hotel1", "" });

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "ImagenUrl" },
                values: new object[] { new DateTime(2025, 4, 15, 15, 44, 27, 234, DateTimeKind.Local).AddTicks(5249), "hotel2", "" });

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaCreacion", "ImagenUrl" },
                values: new object[] { new DateTime(2025, 4, 15, 15, 44, 27, 234, DateTimeKind.Local).AddTicks(5251), "hotel3", "" });
        }
    }
}
