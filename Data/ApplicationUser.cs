using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SSocial.Data
{
    public class ApplicationUser : IdentityUser
    {
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}