using System.ComponentModel.DataAnnotations;

namespace MoneyApp.Server.Models
{
    public class Category
    {
        [Key]
        public int CategoryId {  get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
