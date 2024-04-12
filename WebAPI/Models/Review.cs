using WebAPI.Models;

namespace Review_RatingAPI.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ListingId { get; set; }
        public string Content { get; set; } = string.Empty; //Ensure non-null by default
        public double Rating { get; set; }
        public DateTime ReviewTime { get; set; }

        // Navigation property
        public virtual Users User { get; set; }
    }
}
