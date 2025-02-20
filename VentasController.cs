using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VentasAPI.Data;
using VentasAPI.Models;

namespace VentasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly VentasContext _context;

        public VentasController(VentasContext context)
        {
            _context = context;
        }

        // GET: api/Ventas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
        {
            return await _context.Ventas.ToListAsync();
        }

        // GET: api/Ventas/5
        [HttpGet("{folio}")]
        public async Task<ActionResult<Venta>> GetVenta(string folio)
        {
            var venta = await _context.Ventas.FindAsync(folio);

            if (venta == null)
            {
                return NotFound();
            }

            return venta;
        }

        // PUT: api/Ventas/5
        [HttpPut("{folio}")]
        public async Task<IActionResult> PutVenta(string folio, Venta venta)
        {
            if (folio != venta.Folio)
            {
                return BadRequest();
            }

            _context.Entry(venta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VentaExists(folio))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ventas
        [HttpPost]
        public async Task<ActionResult<Venta>> PostVenta(Venta venta)
        {
            _context.Ventas.Add(venta);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VentaExists(venta.Folio))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetVenta", new { folio = venta.Folio }, venta);
        }

        // DELETE: api/Ventas/5
        [HttpDelete("{folio}")]
        public async Task<IActionResult> DeleteVenta(string folio)
        {
            var venta = await _context.Ventas.FindAsync(folio);
            if (venta == null)
            {
                return NotFound();
            }

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Ventas/OrderByTotal
        [HttpGet("OrderByTotal")]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentasOrderedByTotal()
        {
            return await _context.Ventas.OrderByDescending(v => v.Total).ToListAsync();
        }

        // GET: api/Ventas/MayoresA1000
        [HttpGet("MayoresA1000")]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentasMayoresA1000()
        {
            return await _context.Ventas.Where(v => v.Total > 1000).ToListAsync();
        }

        // GET: api/Ventas/PorAnio/2023
        [HttpGet("PorAnio/{anio}")]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentasPorAnio(int anio)
        {
            return await _context.Ventas.Where(v => v.Fecha.Year == anio).ToListAsync();
        }

        // GET: api/Ventas/MayorImporte
        [HttpGet("MayorImporte")]
        public async Task<ActionResult<Venta>> GetVentaMayorImporte()
        {
            return await _context.Ventas.OrderByDescending(v => v.Total).FirstOrDefaultAsync();
        }

        private bool VentaExists(string folio)
        {
            return _context.Ventas.Any(e => e.Folio == folio);
        }
    }
}

