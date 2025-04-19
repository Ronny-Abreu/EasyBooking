using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCampoServicios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Servicios",
                table: "Hoteles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "Servicios" },
                values: new object[] { new DateTime(2025, 4, 15, 15, 9, 47, 186, DateTimeKind.Local).AddTicks(2731), "" });

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "Servicios" },
                values: new object[] { new DateTime(2025, 4, 15, 15, 9, 47, 186, DateTimeKind.Local).AddTicks(2754), "" });

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaCreacion", "Servicios" },
                values: new object[] { new DateTime(2025, 4, 15, 15, 9, 47, 186, DateTimeKind.Local).AddTicks(2756), "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Servicios",
                table: "Hoteles");

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 14, 12, 21, 39, 952, DateTimeKind.Local).AddTicks(4279));

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 14, 12, 21, 39, 952, DateTimeKind.Local).AddTicks(4323));

            migrationBuilder.UpdateData(
                table: "Hoteles",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 4, 14, 12, 21, 39, 952, DateTimeKind.Local).AddTicks(4327));
        }
    }
}
