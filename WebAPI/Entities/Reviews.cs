
namespace WebAPI.Entities
{
    public class Reviews
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty; //Ensure non-null by default
        public double Rating { get; set; }
        public DateTime ReviewTime { get; set; }

        // Navigation property

        public User User { get; set; }
        public string Username { get; set; } = string.Empty; //make it non-nullable to fix the warning
    }
}
