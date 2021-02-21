using System.ComponentModel.DataAnnotations;

namespace SSocial.Dtos
{
    public class LoginUserDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
    
    public class RegisterUserDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
    }
}