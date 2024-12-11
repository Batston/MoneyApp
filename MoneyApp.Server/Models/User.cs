using System.ComponentModel.DataAnnotations;

namespace MoneyApp.Server.Models
{
    public class User
    {
        [Key]
        public int UserId {  get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
