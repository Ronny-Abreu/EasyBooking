using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CampoVerificarEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailValidado",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7440));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7442));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7443));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7445));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7451));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7452));

            migrationBuilder.UpdateData(
                table: "HotelImagenes",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7454));

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7293));

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7308));

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7509));

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7511));

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7512));

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7513));

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7514));

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7516));

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7517));

            migrationBuilder.UpdateData(
                table: "PaquetesTuristicos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7484));

            migrationBuilder.UpdateData(
                table: "PaquetesTuristicos",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7491));

            migrationBuilder.UpdateData(
                table: "PaquetesTuristicos",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 19, 22, 8, 40, 979, DateTimeKind.Local).AddTicks(7493));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailValidado",
                table: "Usuarios");

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

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5528));

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5530));

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5531));

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5533));

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5539));

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5541));

            migrationBuilder.UpdateData(
                table: "PaqueteImagenes",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5542));

            migrationBuilder.UpdateData(
                table: "PaquetesTuristicos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5502));

            migrationBuilder.UpdateData(
                table: "PaquetesTuristicos",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5506));

            migrationBuilder.UpdateData(
                table: "PaquetesTuristicos",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 18, 22, 44, 36, 437, DateTimeKind.Local).AddTicks(5509));
        }
    }
}
