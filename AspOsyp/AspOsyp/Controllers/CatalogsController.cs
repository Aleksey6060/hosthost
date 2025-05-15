using AspOsyp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspOsyp.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        private readonly SaitASPosipyan2Context _context;

        public CatalogsController(SaitASPosipyan2Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCatalogs()
        {
            var catalogs = await _context.Catalogs
                .Select(c => new
                {
                    c.ProductId,
                    c.ProductName
                })
                .ToListAsync();

            return Ok(catalogs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Catalog>> GetCatalog(int id)
        {
            var catalog = await _context.Catalogs.FindAsync(id);

            if (catalog == null)
            {
                return NotFound();
            }

            return catalog;
        }

        [HttpPost]
        public async Task<ActionResult<Catalog>> PostCatalog(Catalog catalog)
        {
            _context.Catalogs.Add(catalog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCatalog), new { id = catalog.ProductId }, catalog);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCatalog(int id, Catalog catalog)
        {
            if (id != catalog.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(catalog).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCatalog(int id)
        {
            var catalog = await _context.Catalogs.FindAsync(id);
            if (catalog == null)
            {
                return NotFound();
            }

            _context.Catalogs.Remove(catalog);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}