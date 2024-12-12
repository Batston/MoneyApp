using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MoneyApp.Server.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        [Required]
        public int WalletID { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        [Required]
        public decimal Amount { get; set; }
        [AllowNull]
        public string Description { get; set; }
    }
}
