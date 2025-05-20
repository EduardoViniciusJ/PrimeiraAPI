using System.ComponentModel.DataAnnotations;

namespace PrimeiraAPI.DTOs
{
    public class LoginModelDTO
    {
        [Required(ErrorMessage ="User name is requered")]
        public string? UserName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage ="Email is requered")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is requered")]
        public string? Passsword { get; set; }
    }
}
