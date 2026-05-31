using Microsoft.EntityFrameworkCore;
using EcoSystem.Data.Models;

namespace EcoSystem.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Aquí registramos las tablas que existirán en tu base de datos
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
    }
}