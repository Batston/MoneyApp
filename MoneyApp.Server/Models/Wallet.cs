using System.ComponentModel.DataAnnotations;

namespace MoneyApp.Server.Models
{
    public class Wallet
    {
        [Key]
        public int WalletID { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string WalletName { get; set; }  
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "VND";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
