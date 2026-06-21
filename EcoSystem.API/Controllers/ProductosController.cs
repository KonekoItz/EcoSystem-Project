using EcoSystem.Business.Interfaces;
using EcoSystem.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcoSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var productos = await _productoService.ObtenerTodosAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _productoService.ObtenerPorIdAsync(id);
            if (producto == null) return NotFound(new { mensaje = "Producto no encontrado." });
            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            var nuevoProducto = await _productoService.CrearAsync(producto);
            return CreatedAtAction(nameof(GetProducto), new { id = nuevoProducto.Id }, nuevoProducto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            var actualizado = await _productoService.ActualizarAsync(id, producto);
            if (!actualizado) return BadRequest(new { mensaje = "Error al actualizar o el ID no coincide." });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var eliminado = await _productoService.EliminarAsync(id);
            if (!eliminado) return NotFound(new { mensaje = "El producto que intentas eliminar no fue encontrado." });
            return NoContent();
        }
    }
}