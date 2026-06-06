namespace EcoSystem.Data.Models
{
    public class DetalleOrden
    {
        public int Id { get; set; }

        // Llaves foráneas
        public int OrdenId { get; set; }
        public int ProductoId { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        // Propiedades de navegación
        public Orden? Orden { get; set; }
        public Producto? Producto { get; set; }
    }
}