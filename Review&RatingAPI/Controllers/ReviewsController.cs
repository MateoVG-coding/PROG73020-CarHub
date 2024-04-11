using Microsoft.AspNetCore.Mvc;
using Review_RatingAPI.Models;

namespace Review_RatingAPI.Controllers
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
            reviews.Add(review);
            return Ok();
        }
    }
    //[other api methods]
}
