using System.ComponentModel.DataAnnotations;

namespace Core.Auth
{
    public class RegisterDto
    {
        [Required]
        public string NameSurname { get; set; }

        [Required]
        public string Username { get; set; }       

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords Are Not Same")]
        public string ConfirmPassword { get; set; }
    }
}
