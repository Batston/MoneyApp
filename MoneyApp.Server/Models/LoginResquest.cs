using System.ComponentModel.DataAnnotations;

namespace MoneyApp.Server.Models
{
    public class LoginResquest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
