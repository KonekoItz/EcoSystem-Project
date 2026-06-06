using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcoSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class DatosPruebaReportes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "Ciudad", "Email", "FechaRegistro", "Nombre" },
                values: new object[,]
                {
                    { 1, "CDMX", "ana@email.com", new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Ana Garcia" },
                    { 2, "Guadalajara", "luis@email.com", new DateTime(2023, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Luis Martínez" }
                });

            migrationBuilder.InsertData(
                table: "Ordenes",
                columns: new[] { "Id", "ClienteId", "Estado", "FechaOrden" },
                values: new object[] { 1, 1, "entregado", new DateTime(2024, 1, 10, 10, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "DetallesOrdenes",
                columns: new[] { "Id", "Cantidad", "OrdenId", "PrecioUnitario", "ProductoId" },
                values: new object[] { 1, 2, 1, 45.99m, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DetallesOrdenes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ordenes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
