using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSocial.Configuration;

namespace SSocial.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public List<RefreshToken> RefreshTokens { get; set; }
        public string Name { get; set; }
        
        public string Role { get; set; }
    }
    
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole(string roleName) : base(roleName)
        {
        }
        public ApplicationRole()
        { }
    }
}