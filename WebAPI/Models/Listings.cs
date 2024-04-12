namespace WebAPI.Models
{
    public class Listings
    {
        public int listingId { get; set; }

        public Cars Car { get; set; }

        public Users User { get; set; }

        public int Value { get; set; }

        public string? Description { get; set; }
    }
}
