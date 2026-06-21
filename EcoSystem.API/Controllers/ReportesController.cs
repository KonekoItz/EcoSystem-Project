using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoSystem.Data;

namespace EcoSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportesController(AppDbContext context)
        {
            _context = context;
        }

        // REPORTE 1: INNER JOIN (Clientes con sus órdenes)
        [HttpGet("clientes-ordenes")]
        public async Task<IActionResult> GetClientesConOrdenes()
        {
            var reporte = await _context.Ordenes
                .Include(o => o.Cliente)
                .Select(o => new
                {
                    Cliente = o.Cliente!.Nombre,
                    Ciudad = o.Cliente.Ciudad,
                    IdOrden = o.Id,
                    FechaOrden = o.FechaOrden,
                    Estado = o.Estado
                })
                .ToListAsync();

            return Ok(reporte);
        }

        // REPORTE 2: LEFT JOIN (Todos los clientes, tengan o no órdenes)
        [HttpGet("clientes-resumen-left-join")]
        public async Task<IActionResult> GetClientesResumen()
        {
            // Proyectar desde la tabla izquierda hacia subconsultas de la derecha 
            // obliga a Entity Framework a generar un comportamiento de LEFT JOIN en SQL.
            var reporte = await _context.Clientes
                .Select(c => new
                {
                    Cliente = c.Nombre,
                    Ciudad = c.Ciudad,
                    TotalOrdenes = _context.Ordenes.Count(o => o.ClienteId == c.Id),
                    UltimaOrden = _context.Ordenes
                        .Where(o => o.ClienteId == c.Id)
                        .OrderByDescending(o => o.FechaOrden)
                        .Select(o => (DateTime?)o.FechaOrden)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(reporte);
        }
    }
}