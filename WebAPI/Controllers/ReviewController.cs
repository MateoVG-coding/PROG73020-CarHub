using Microsoft.AspNetCore.Mvc;
using Review_RatingAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private static List<Review> reviews = new List<Review>();

        [HttpGet]
        public ActionResult<List<Review>> GetAllReviews()
        {
            return reviews;
        }

        [HttpPost]
        public IActionResult CreateReview([FromBody] Review review)
        {
            if (review == null)
            {
                return BadRequest("Review cannot be empty");
            }
            reviews.Add(review);
            return Ok();
        }
    }
}
