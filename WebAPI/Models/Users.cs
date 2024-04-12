using Microsoft.AspNetCore.Identity;

namespace WebAPI.Models
{
    public class Users : IdentityUser
    {
        public int UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Username { get; set; }


        // TO DO: Waiting for implementation of Rating and Review classes

        // public Ratings Rating { get; set; }

        // public Reviews Reviews { get; set; }
    }
}
