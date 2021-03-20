using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSocial.Configuration;
using SSocial.Models;

namespace SSocial.Data
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser<Guid>
    {
        public List<RefreshToken> RefreshTokens { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
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