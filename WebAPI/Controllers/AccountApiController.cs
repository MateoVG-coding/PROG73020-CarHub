using Microsoft.AspNetCore.Mvc;
using WebAPI.Messages;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController()]
    public class AccountApiController : Controller
    {
        private readonly IAuthService _authService;

        public AccountApiController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("/api/register")]
        public async Task<IActionResult> RegisterUser(UserRegistrationRequest userRegistrationRequest)
        {
            var result = await _authService.RegisterUser(userRegistrationRequest);

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
            bool isSuccessful = await _authService.LoginUser(loginRequest);

            if (isSuccessful)
            {
                return Ok(new { Message = "Authentication successful." });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }
        }
    }
}
