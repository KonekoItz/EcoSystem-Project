namespace EcoSystem.Data.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Ciudad { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Propiedad de navegación: Un cliente puede tener muchas órdenes
        public ICollection<Orden>? Ordenes { get; set; }
    }
}