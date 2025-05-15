using AspOsyp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspOsyp.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly SaitASPosipyan2Context _context;

        public CartsController(SaitASPosipyan2Context context)
        {
            _context = context;
        }

        // GET: api/carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCarts()
        {
            var carts = await _context.Carts
                .Select(c => new
                {
                    c.CartId,
                    c.UserId,
                    c.ProductId,
                    c.Quantity
                })
                .ToListAsync();
            return Ok(carts);
        }

        // GET: api/carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart([FromBody] CartCreateDto cartDto)
        {
            if (cartDto == null || cartDto.UserId <= 0 || cartDto.ProductId <= 0 || cartDto.Quantity <= 0)
            {
                return BadRequest("Invalid cart data: UserId, ProductId, and Quantity must be positive integers.");
            }

            // Проверка существования внешних ключей
            if (!_context.Users.Any(u => u.UserId == cartDto.UserId))
            {
                return BadRequest($"User with ID {cartDto.UserId} does not exist.");
            }
            if (!_context.Catalogs.Any(c => c.ProductId == cartDto.ProductId))
            {
                return BadRequest($"Product with ID {cartDto.ProductId} does not exist.");
            }

            var cart = new Cart
            {
                UserId = cartDto.UserId,
                ProductId = cartDto.ProductId,
                Quantity = cartDto.Quantity
            };

            try
            {
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCart), new { id = cart.CartId }, cart);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Failed to save cart: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // PUT: api/carts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}