using Microsoft.EntityFrameworkCore;
using EcoSystem.Data.Models;

namespace EcoSystem.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

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
        }
    }
}