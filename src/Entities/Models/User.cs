using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public List<RefreshToken> RefreshTokens { get; set; }
    }
    
    public class Role : IdentityRole<Guid>
    {
        public Role(string roleName) : base(roleName)
        {
        }
        public Role()
        { }
        
        public static string Admin = "Admin";
        public static string Supervisor = "Supervisor";
        public static string Mechanic = "Mechanic";
    }
}