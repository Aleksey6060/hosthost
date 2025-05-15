using AspOsyp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Добавлено для Include

namespace AspOsyp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly SaitASPosipyan2Context _context;

        public ReviewsController(SaitASPosipyan2Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews(int? productId)
        {
            var query = _context.Reviews.Include(r => r.User).AsQueryable();
            if (productId.HasValue)
            {
                query = query.Where(r => r.ProductId == productId.Value);
            }
            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetReviews", new { id = review.ReviewId }, review);
        }
    }
}