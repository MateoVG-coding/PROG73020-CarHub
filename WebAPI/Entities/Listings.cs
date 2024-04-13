namespace WebAPI.Entities
{
    public class Listings
    {
        public int listingId { get; set; }

        public Cars Car { get; set; }

        public User User { get; set; }

        public int Value { get; set; }

        public string? Description { get; set; }
    }
}
