namespace EcoSystem.Data.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; } // Llave foránea
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Sku { get; set; }

        // Propiedad de navegación para Entity Framework
        public Categoria Categoria { get; set; } = null!;
    }
}