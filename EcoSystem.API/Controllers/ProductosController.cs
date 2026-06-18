using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoSystem.API.Data;
using EcoSystem.Data.Models;
using System;

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
        public async Task<IActionResult> GetProductos()
        {
            try
            {
                // Va a Supabase, busca los productos, incluye los datos de su Categoría
                var productos = await _context.Productos
                                              .Include(p => p.Categoria)
                                              .ToListAsync();

                if (productos.Count == 0)
                {
                    // Formato Material 7: Respuesta estructurada para 404
                    return NotFound(new { mensaje = "No se encontraron productos", datos = new List<Producto>() });
                }

                // Formato Material 7: Respuesta estructurada para 200 OK
                return Ok(new
                {
                    mensaje = "Productos obtenidos correctamente",
                    total = productos.Count,
                    datos = productos
                });
            }
            catch (Exception ex)
            {
                // Formato Material 7: Protección contra caídas del servidor
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        // GET: api/productos/buscar?texto=sensor
        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarProductos([FromQuery] string texto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(texto))
                {
                    return BadRequest(new { mensaje = "El texto de búsqueda no puede estar vacío." });
                }

                var productos = await _context.Productos
                                              .Include(p => p.Categoria)
                                              .Where(p => p.Nombre.ToLower().Contains(texto.ToLower()) ||
                                                          p.Descripcion!.ToLower().Contains(texto.ToLower()))
                                              .ToListAsync();

                if (productos.Count == 0)
                {
                    return NotFound(new { mensaje = "No se encontraron productos con ese término." });
                }

                return Ok(new
                {
                    mensaje = "Búsqueda exitosa",
                    total = productos.Count,
                    datos = productos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al buscar productos", error = ex.Message });
            }
        }

        // POST: api/productos
        [HttpPost]
        public async Task<IActionResult> PostProducto(Producto producto)
        {
            try
            {
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();

                // Devolvemos el JSON estructurado confirmando la creación
                return CreatedAtAction(nameof(GetProductos), new { id = producto.Id }, new
                {
                    mensaje = "Producto creado exitosamente",
                    datos = producto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al crear el producto", error = ex.Message });
            }
        }

        // PUT: api/productos/2
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest(new { mensaje = "El ID de la ruta no coincide con el ID del producto." });
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                // 204 No Content no lleva cuerpo JSON, así que lo dejamos tal cual
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Productos.Any(e => e.Id == id))
                {
                    return NotFound(new { mensaje = "El producto que intentas actualizar ya no existe." });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar", error = ex.Message });
            }
        }

        // DELETE: api/productos/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null)
                {
                    return NotFound(new { mensaje = "El producto que intentas eliminar no fue encontrado." });
                }

                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al intentar eliminar el producto", error = ex.Message });
            }
        }
    }
}