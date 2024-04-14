using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using WebAPI.Entities;
using WebAPI.Messages;
using WebAPI.Services;


namespace WebAPI.Controllers
{
    [ApiController()]
    public class AccountApiController : Controller
    {
        public AccountApiController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("/api/register")]
        public async Task<IActionResult> RegisterUser(UserRegistrationRequest userRegistrationRequest)
        {
            var result = await _authenticationService.RegisterUser(userRegistrationRequest);

            if (result.Succeeded)
            {
                return StatusCode(201);
            }
            else
            {
                foreach (var err in result.Errors)
                {
                    ModelState.TryAddModelError(err.Code, err.Description);
                }

                return BadRequest(ModelState);
            }
        }

        [HttpPost("/api/login")]
        public async Task<IActionResult> LoginUser(UserLoginRequest loginRequest)
        {
            bool isSuccessful = await _authenticationService.LoginUser(loginRequest);

            if (isSuccessful)
            {
                return Ok(new { Token = await _authenticationService.CreateToken(true) });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("/api/refresh-token")]
        public async Task<IActionResult> ProcessTokenRefresh([FromBody]TokenInfo tokenInfo)
        {
            TokenInfo refreshedToken = await _authenticationService.RefreshToken(tokenInfo);
            return Ok(refreshedToken);
        }

        private IAuthService _authenticationService;
    }
}
