using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoSystem.API.Data;
using EcoSystem.Data.Models;

namespace EcoSystem.API.Controllers
{
    // Esta será la ruta web: localhost:puerto/api/productos
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        // El constructor recibe la conexión a la base de datos
        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            // Va a Supabase, busca los productos, incluye los datos de su Categoría y los devuelve en una Lista
            var productos = await _context.Productos
                                          .Include(p => p.Categoria)
                                          .ToListAsync();

            return Ok(productos);
        }
        // GET: api/productos/buscar?texto=sensor
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<Producto>>> BuscarProductos([FromQuery] string texto)
        {
            // Validamos que el usuario haya escrito algo
            if (string.IsNullOrWhiteSpace(texto))
            {
                return BadRequest("El texto de búsqueda no puede estar vacío.");
            }

            // APLICANDO LINQ (Clase 2): Filtramos directamente en la base de datos en la nube
            // usando .Where() y .Contains() para coincidencias parciales
            var productos = await _context.Productos
                                          .Include(p => p.Categoria)
                                          .Where(p => p.Nombre.ToLower().Contains(texto.ToLower()) ||
                                                      p.Descripcion!.ToLower().Contains(texto.ToLower()))
                                          .ToListAsync();

            if (productos.Count == 0)
            {
                return NotFound("No se encontraron productos con ese término.");
            }

            return Ok(productos);
        }
        // POST: api/productos
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            // Le decimos a Entity Framework que agregue este nuevo producto
            _context.Productos.Add(producto);

            // Guardamos los cambios para que viajen hasta Supabase
            await _context.SaveChangesAsync();

            // Devuelve un código 201 (Created) confirmando que se guardó exitosamente
            return CreatedAtAction(nameof(GetProductos), new { id = producto.Id }, producto);
        }
        // PUT: api/productos/2
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            // Verificamos que el ID de la URL coincida con el ID del cuerpo (JSON)
            if (id != producto.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del producto.");
            }

            // Le decimos a Entity Framework que este producto ha sido modificado
            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Si alguien borró el producto antes de que pudieras actualizarlo
                if (!_context.Productos.Any(e => e.Id == id))
                {
                    return NotFound("El producto que intentas actualizar ya no existe.");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve un 204 (No Content) que significa: "Todo salió bien, pero no tengo nada nuevo que mostrarte"
            return NoContent();
        }

        // DELETE: api/productos/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            // Primero buscamos si el producto existe
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound("El producto que intentas eliminar no fue encontrado.");
            }

            // Le decimos a la base de datos que lo borre
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            // Devuelve un 204 confirmando la eliminación
            return NoContent();
        }
    }
}