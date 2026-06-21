using Microsoft.EntityFrameworkCore;
using EcoSystem.Data.Models;

namespace EcoSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        // --- TUS 3 NUEVAS TABLAS ---
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<DetalleOrden> DetallesOrdenes { get; set; }

        // Aquí inyectamos los datos de prueba (Seed Data)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Insertamos las Categorías base
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nombre = "Tecnología Verde", Descripcion = "Dispositivos de bajo consumo y soluciones loT sustentables.", Activo = true },
                new Categoria { Id = 2, Nombre = "Energía Renovable", Descripcion = "Paneles solares, inversores y almacenamiento de energía.", Activo = true }
            );

            // Insertamos el Producto vinculado a la Categoría 1
            modelBuilder.Entity<Producto>().HasData(
                new Producto
                {
                    Id = 1,
                    CategoriaId = 1,
                    Nombre = "Sensor IoT Humedad v2",
                    Descripcion = "Sensor de bajo consumo para monitoreo de suelos agrícolas.",
                    Precio = 45.99m,
                    Stock = 120,
                    Sku = "ECO-IOT-HUM-02"
                }
            );

            // --- NUEVOS DATOS DE PRUEBA (CLIENTES, ORDENES Y DETALLES) ---

            // Insertamos 2 Clientes
            modelBuilder.Entity<Cliente>().HasData(
                new Cliente { Id = 1, Nombre = "Ana Garcia", Email = "ana@email.com", Ciudad = "CDMX", FechaRegistro = new DateTime(2023, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
                new Cliente { Id = 2, Nombre = "Luis Martínez", Email = "luis@email.com", Ciudad = "Guadalajara", FechaRegistro = new DateTime(2023, 3, 22, 0, 0, 0, DateTimeKind.Utc) }
            );

            // Insertamos 1 Orden de compra para Ana
            modelBuilder.Entity<Orden>().HasData(
                new Orden { Id = 1, ClienteId = 1, FechaOrden = new DateTime(2024, 1, 10, 10, 30, 0, DateTimeKind.Utc), Estado = "entregado" }
            );

            // Ana compró 2 Sensores IoT en esa orden
            modelBuilder.Entity<DetalleOrden>().HasData(
                new DetalleOrden { Id = 1, OrdenId = 1, ProductoId = 1, Cantidad = 2, PrecioUnitario = 45.99m }
            );
        }
    }
}