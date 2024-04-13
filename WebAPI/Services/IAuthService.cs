﻿using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using WebAPI.Messages;

namespace WebAPI.Services
{
    public interface IAuthService
    {
        public Task<IdentityResult> RegisterUser(UserRegistrationRequest userRegistrationRequest);
        public Task<bool> LoginUser(UserLoginRequest loginRequest);
        public Task<TokenInfo> CreateToken(bool populateExpiry);

        public Task<TokenInfo> RefreshToken(TokenInfo tokenInfo);
    }
}