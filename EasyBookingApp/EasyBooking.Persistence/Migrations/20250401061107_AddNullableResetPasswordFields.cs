using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableResetPasswordFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "ResetCode",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetCodeExpiry",
                table: "Usuarios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Apellido", "AvatarUrl", "Email", "FechaCreacion", "FechaModificacion", "IsEmailVerified", "Nombre", "Password", "ResetCode", "ResetCodeExpiry", "Telefono", "Username" },
                values: new object[] { 20, true, "Admin", null, "admin@easybooking.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Admin2", "hashedpassword", null, null, "1234567890", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DropColumn(
                name: "ResetCode",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ResetCodeExpiry",
                table: "Usuarios");

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Apellido", "AvatarUrl", "Email", "FechaCreacion", "FechaModificacion", "IsEmailVerified", "Nombre", "Password", "Telefono", "Username" },
                values: new object[] { 1, true, "Admin", null, "admin@easybooking.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Admin", "hashedpassword", "1234567890", "admin" });
        }
    }
}
