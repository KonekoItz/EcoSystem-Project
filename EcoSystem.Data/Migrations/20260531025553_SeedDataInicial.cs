using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcoSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Activo", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Dispositivos de bajo consumo y soluciones loT sustentables.", "Tecnología Verde" },
                    { 2, true, "Paneles solares, inversores y almacenamiento de energía.", "Energía Renovable" }
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "CategoriaId", "Descripcion", "Nombre", "Precio", "Sku", "Stock" },
                values: new object[] { 1, 1, "Sensor de bajo consumo para monitoreo de suelos agrícolas.", "Sensor IoT Humedad v2", 45.99m, "ECO-IOT-HUM-02", 120 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
