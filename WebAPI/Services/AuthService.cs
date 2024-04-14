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
    public class AuthService
    {
        public AuthService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
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

            if (result.Succeeded)
            {
                result = await _userManager.AddToRolesAsync(user, userRegistrationRequest.Roles);
            }

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

        public async Task<TokenInfo> CreateToken(bool populateExpiry)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = GenerateRefreshToken();

            _user.RefreshToken = refreshToken;

            if (populateExpiry)
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(30);

            await _userManager.UpdateAsync(_user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenInfo() { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public async Task<TokenInfo> RefreshToken(TokenInfo token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null || user.RefreshToken != token.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new SecurityTokenException("Token has invalid values");

            _user = user;

            return await CreateToken(false);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))),
                ValidateLifetime = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secretKeyText = Environment.GetEnvironmentVariable("SECRET");

            var key = Encoding.UTF8.GetBytes(secretKeyText);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }


        private User? _user;

        private UserManager<User> _userManager;

        private IConfiguration _configuration;
    }
}
