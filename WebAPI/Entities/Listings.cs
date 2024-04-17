using Microsoft.Identity.Client;

namespace WebAPI.Entities
{
    public class Listings
    {
        public int listingsId { get; set; }

        public DateTime PostingDate { get; set; }

        public int CarId { get; set; }

        public Cars Car { get; set; }

        public string Username { get; set; } = string.Empty;

        public User User { get; set; }

        public int Value { get; set; }

        public string? Description { get; set; }
    }
}
