using WebAPI.Messages;

namespace WebAPI.Entities
{
    public class ListingApiHomeDTO
    {
        public Dictionary<string, Link>? Links { get; set; }

        public string? ApiVersion { get; set; }

        public string? Creator { get; set; }
    }
}
