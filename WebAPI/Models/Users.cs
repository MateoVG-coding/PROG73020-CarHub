using Microsoft.AspNetCore.Identity;

namespace WebAPI.Models
{
    public class Users : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
