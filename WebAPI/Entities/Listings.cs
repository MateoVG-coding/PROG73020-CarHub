namespace WebAPI.Entities
{
    public class Listings
    {
        public int listingId { get; set; }

        public DateTime PostingDate { get; set; }

        public int CarID { get; set; }

        public string Username { get; set; } = String.Empty;

        public int Value { get; set; }

        public string? Description { get; set; }
    }
}
