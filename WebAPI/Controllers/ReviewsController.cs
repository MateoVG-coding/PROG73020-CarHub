using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Messages;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly ListingDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SessionManager _sessionManager;

        public ReviewsController(ListingDbContext context, IHttpContextAccessor httpContextAccessor, SessionManager sessionManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _sessionManager = sessionManager;

        }

        [HttpGet]
        public async Task<ActionResult<List<Reviews>>> GetAllReviews(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                return await _context.Reviews.Where(r => r.Username == username).ToListAsync();
            }
            else
            {
                return await _context.Reviews.ToListAsync();
            }
        }

        //search a review
        [HttpGet("{id}")]
        public async Task<ActionResult<Reviews>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return review;
        }

        //post a review
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] Reviews review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = _sessionManager.getSessionId();

            // Check if the user ID cookie exists
            if (string.IsNullOrEmpty(userId))
            {
                // Handle the case where the user ID cookie is missing or invalid
                return Unauthorized();
            }

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        //update a review
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] Reviews review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            var userId = _sessionManager.getSessionId();

            // Check if the user ID cookie exists
            if (string.IsNullOrEmpty(userId))
            {
                // Handle the case where the user ID cookie is missing or invalid
                return Unauthorized();
            }

            _context.Entry(review).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Reviews.Any(r => r.Id == id))
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

        //delete a review
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            var userId = _sessionManager.getSessionId();

            // Check if the user ID cookie exists
            if (string.IsNullOrEmpty(userId))
            {
                // Handle the case where the user ID cookie is missing or invalid
                return Unauthorized();
            }

            if (review == null)
            {
                return NotFound();
            }
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}