using AspOsyp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspOsyp.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly SaitASPosipyan2Context _context;

        public OrderItemsController(SaitASPosipyan2Context context)
        {
            _context = context;
        }

        // GET: api/orderitems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetOrderItems()
        {
            var orderItems = await _context.OrderItems
                .Select(oi => new
                {
                    oi.OrderItemsId,
                    oi.OrderId,
                    oi.ProductId,
                    oi.Quantity,
                    oi.Price
                })
                .ToListAsync();
            return Ok(orderItems);
        }

        // GET: api/orderitems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return orderItem;
        }

        // POST: api/orderitems
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderItem), new { id = orderItem.OrderItemsId }, orderItem);
        }

        // PUT: api/orderitems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderItem orderItem)
        {
            if (id != orderItem.OrderItemsId)
            {
                return BadRequest();
            }

            _context.Entry(orderItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/orderitems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}