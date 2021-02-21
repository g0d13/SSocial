using System.ComponentModel.DataAnnotations;

namespace SSocial.Models
{
    public class UserDetails
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        
        public string Role { get; set; }
    }
}