﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebAPI.Entities;
using WebAPI.Messages;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController()]
    public class AccountApiController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ListingDbContext _dBcontext;

        public AccountApiController(IAuthService authService, IHttpContextAccessor httpContextAccessor, ListingDbContext context)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _dBcontext = context;
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
                // Store user ID in a cookie
                var user = await _dBcontext.Users.FirstOrDefaultAsync(u => u.UserName == loginRequest.UserName) ; // Assuming AuthService has a method to get the user ID

                if (user == null)
                {
                    return NotFound(); // User with the given username not found
                }

                var cookieOptions = new CookieOptions();

                cookieOptions.Expires = DateTimeOffset.Now.AddHours(1);
                cookieOptions.Path = "/";

                HttpContext.Response.Cookies.Append("UserId", user.Id.ToString(), cookieOptions);

                return Ok(new { Message = "Authentication successful." });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }
        }
    }
}
