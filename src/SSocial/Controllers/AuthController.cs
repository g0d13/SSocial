using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using Entities.Configuration;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;
        private readonly UserManager<User> _userManager;
        private readonly RepositoryContext _dbContext;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger _logger;
        
        private readonly IMapper _mapper;

        public AuthController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, 
            UserManager<User> userManager, 
            RepositoryContext dbContext,
            ILogger<AuthController> logger,
            RoleManager<Role> roleManager,
            IMapper mapper)
        {
            _jwtBearerTokenSettings = jwtTokenOptions.Value;
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            _logger.LogInformation("Registering user");
            if (!ModelState.IsValid || registerUserDto == null)
            {
                return new BadRequestObjectResult(new {Message = "User Registration Failed"});
            }

            var identityUser = _mapper.Map<User>(registerUserDto);

            var result = await _userManager.CreateAsync(identityUser, registerUserDto.Password);
            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (var error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }

                return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = dictionary });
            }

            var role = _roleManager.Roles.FirstOrDefault(e => e.Name == registerUserDto.Role);
            if (role == null)
            {
                return new BadRequestObjectResult(new { Message = "Role was not found" });
            }
            
            //Get the created user from the database
            var user = await _userManager.FindByIdAsync(identityUser.Id.ToString());
            await _userManager.AddToRoleAsync(user, role.Name);

            return Ok(new { Message = "User Registration Successful" });
        }

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            User identityUser;

            if (!ModelState.IsValid
                || userDto == null
                || (identityUser = await ValidateUser(userDto)) == null)
            {
                return new BadRequestObjectResult(new { Message = "Usuario y/o contrasena incorrecto" });
            }

            var token = await GenerateTokens(identityUser);
            return Ok(new { Token = token, Message = "Success" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        public IActionResult RefreshToken()
        {
            var token = HttpContext.Request.Cookies["refreshToken"];
            var identityUser = _dbContext.Users.Include(x => x.RefreshTokens)
                .FirstOrDefault(x => x.RefreshTokens.Any(y => y.Token == token && y.UserId == x.Id));

            // Get existing refresh token if it is valid and revoke it
            var existingRefreshToken = GetValidRefreshToken(token, identityUser);
            if(existingRefreshToken == null)
            {
                return new BadRequestObjectResult(new { Message = "Failed" });
            }

            if (HttpContext.Connection.RemoteIpAddress != null)
                existingRefreshToken.RevokedByIp = HttpContext.Connection.RemoteIpAddress.ToString();
            existingRefreshToken.RevokedOn = DateTime.UtcNow;
            
            
            // Generate new tokens
            var newToken = GenerateTokens(identityUser);
            return Ok(new {Token = newToken, Message = "Success"});
        }

        [HttpPost]
        [Route("RevokeToken")]
        public IActionResult RevokeToken(string token)
        {
            // If user found, then revoke
            if (RevokeRefreshToken(token))
            {
                return Ok(new {Message = "Success"});
            }

            // Otherwise, return error
            return new BadRequestObjectResult(new {Message = "Failed"});
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            // Revoke Refresh Token 
            RevokeRefreshToken();
            return Ok(new {Token = "", Message = "Logged Out"});
        }

        private RefreshToken GetValidRefreshToken(string token, User identityUser)
        {
            if (identityUser == null)
            {
                return null;
            }

            var existingToken = identityUser.RefreshTokens.FirstOrDefault(x => x.Token == token);
            return IsRefreshTokenValid(existingToken) ? existingToken : null;
        }

        private bool RevokeRefreshToken(string token = null)
        {
            token ??= HttpContext.Request.Cookies["refreshToken"];
            var identityUser = _dbContext.Users.Include(x => x.RefreshTokens)
                .FirstOrDefault(x => x.RefreshTokens.Any(y => y.Token == token && y.UserId == x.Id));
            if (identityUser == null)
            {
                return false;
            }

            // Revoke Refresh token
            var existingToken = identityUser.RefreshTokens.FirstOrDefault(x => x.Token == token);
            if (existingToken != null)
            {
                if (HttpContext.Connection.RemoteIpAddress != null)
                    existingToken.RevokedByIp = HttpContext.Connection.RemoteIpAddress.ToString();
                existingToken.RevokedOn = DateTime.UtcNow;
            }

            _dbContext.Update(identityUser);
            _dbContext.SaveChanges();
            return true;
        }

        private async Task<User> ValidateUser(LoginUserDto userDto)
        {
            var identityUser = await _userManager.FindByEmailAsync(userDto.Email);
            if (identityUser == null) return null;
            var result =
                _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash,
                    userDto.Password);
            return result == PasswordVerificationResult.Failed ? null : identityUser;

        }

        private async Task<string> GenerateTokens(User identityUser)
        {
            // Generate access token
            var accessToken = await GenerateAccessToken(identityUser);

            // Generate refresh token and set it to cookie
            if (HttpContext.Connection.RemoteIpAddress != null)
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                var refreshToken = GenerateRefreshToken(ipAddress, identityUser.Id);

                // Set Refresh Token Cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(7)
                };
                HttpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

                // Save refresh token to database
                identityUser.RefreshTokens ??= new List<RefreshToken>();

                identityUser.RefreshTokens.Add(refreshToken);
            }

            _dbContext.Update(identityUser);
            await _dbContext.SaveChangesAsync();
            return accessToken;
        }

        private async Task<string> GenerateAccessToken(User identityUser)
        {
            var getRole = await _userManager.GetRolesAsync(identityUser);
            var role = getRole[0];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtBearerTokenSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, identityUser.Email),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Actor, identityUser.Id.ToString())
                }),

                Expires = DateTime.UtcNow.AddSeconds(_jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtBearerTokenSettings.Audience,
                Issuer = _jwtBearerTokenSettings.Issuer,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress, Guid userId)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                ExpiryOn = DateTime.UtcNow.AddDays(_jwtBearerTokenSettings.RefreshTokenExpiryInDays),
                CreatedOn = DateTime.UtcNow,
                CreatedByIp = ipAddress,
                UserId = userId
            };
        }

        private bool IsRefreshTokenValid(RefreshToken existingToken)
        {
            // Is token already revoked, then return false
            if (existingToken.RevokedByIp != null && existingToken.RevokedOn != DateTime.MinValue)
            {
                return false;
            }

            // Token already expired, then return false
            return existingToken.ExpiryOn > DateTime.UtcNow;
        }
    }
}