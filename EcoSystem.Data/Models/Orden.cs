namespace EcoSystem.Data.Models
{
    public class Orden
    {
        public int Id { get; set; }

        // Llave foránea que lo conecta con el Cliente
        public int ClienteId { get; set; }

        public DateTime FechaOrden { get; set; } = DateTime.UtcNow;
        public string Estado { get; set; } = "pendiente"; // pendiente, enviado, entregado, cancelado

        // Propiedades de navegación (Para que EF Core haga los JOINs mágicamente)
        public Cliente? Cliente { get; set; }
        public ICollection<DetalleOrden>? Detalles { get; set; }
    }
}