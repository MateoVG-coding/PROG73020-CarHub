using Microsoft.AspNetCore.Identity;

namespace WebAPI.Entities
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }


        // TO DO: Waiting for implementation of Rating and Review classes

        // public Ratings Rating { get; set; }

        // public Reviews Reviews { get; set; }public string? FirstName { get; set; }

    }
}
