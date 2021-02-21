using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SSocial.Configuration;

namespace SSocial.Data
{
    public class ApplicationUser : IdentityUser
    {
        public List<RefreshToken> RefreshTokens { get; set; }
        public string Role { get; set; }
    }
}