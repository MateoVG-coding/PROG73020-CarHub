
namespace WebAPI.Entities
{
    public class Reviews
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public int ListingId { get; set; }
        public string Content { get; set; } = string.Empty; //Ensure non-null by default
        public double Rating { get; set; }
        public DateTime ReviewTime { get; set; }

        // Navigation property
        public virtual User? User { get; set; } //make it non-nullable to fix the warning
    }
}
