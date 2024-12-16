using System.ComponentModel.DataAnnotations;

namespace MoneyApp.Server.Models
{
    public class Category
    {
        [Key]
        int CategoryId {  get; set; }
        [Required]
        int UserId { get; set; }
        [Required]
        string CategoryName { get; set; }
        [Required]
        string Type { get; set; }
        [Required]
        DateTime DateTime { get; set; } = DateTime.Now;
    }
}
