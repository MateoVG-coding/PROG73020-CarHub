using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using WebAPI.Entities;
using WebAPI.Messages;

namespace WebAPI.Services
{
    public class AuthService : IAuthService
    {
        public AuthService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterUser(UserRegistrationRequest userRegistrationRequest)
        {
            User user = new User()
            {
                UserName = userRegistrationRequest.UserName,
                Email = userRegistrationRequest.Email,
                PhoneNumber = userRegistrationRequest.PhoneNumber,
                FirstName = userRegistrationRequest.FirstName,
                LastName = userRegistrationRequest.LastName
            };

            var result = await _userManager.CreateAsync(user, userRegistrationRequest.Password);

            return result;
        }

        public async Task<bool> LoginUser(UserLoginRequest loginRequest)
        {
            _user = await _userManager.FindByNameAsync(loginRequest.UserName);
            if (_user == null)
                return false;

            var result = await _userManager.CheckPasswordAsync(_user, loginRequest.Password);
            return result;
        }

        private User? _user;

        private UserManager<User> _userManager;

    }
}
