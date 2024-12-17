using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MoneyApp.Server.Data;
using MoneyApp.Server.Models;
using System.Security.Claims;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Transaction = MoneyApp.Server.Models.Transaction;

namespace MoneyApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        public readonly DBContext _context;
        public TransactionController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTransactions()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserId == null)
                return Unauthorized(new { Message = "Người dùng không hợp lệ" });

            int id = int.Parse(UserId);
            
            var transactionsWithUser = await (from transaction in _context.Transactions
                                              join wallet in _context.Wallets
                                              on transaction.WalletID equals wallet.WalletID
                                              where wallet.UserId == id
                                              select new
                                              {
                                                  Transaction = transaction,
                                              }
                                              ).ToListAsync();
            return Ok(transactionsWithUser);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized(new { Message = "Người dùng không hợp lệ!" });

            int id = int.Parse(userId);

            var isWalletExist = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == id && w.WalletID == transaction.WalletID);

            if (isWalletExist == null)
                return NotFound(new { Message = "Người dùng không được thực hiện giao dịch trên ví này!" });

            var category = await _context.Categories.FindAsync(transaction.CategoryID);

            if (category == null)
                return BadRequest(new { Message = "Danh mục không hợp lệ!" });

            if (category.Type == "Chi")
            { 
               if (isWalletExist.Balance < transaction.Amount)
                return BadRequest(new { Message = "Số dư ví không đủ để thực hiện giao dịch!" });
               else 
                isWalletExist.Balance -= transaction.Amount; 
            }
            else if(category.Type == "Thu")
                isWalletExist.Balance += transaction.Amount;

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return Ok( new { Message = "Thêm giao  dịch thành công!"});
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutTransaction([FromBody] Transaction newTransaction)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (UserId == null)
                return Unauthorized(new { Message = "Người dùng không hợp lệ!" });

            int userId = int.Parse(UserId);

            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionID == newTransaction.TransactionID);

            if (transaction == null)
                return NotFound(new { Message = "Giao dịch không tồn tại!" });

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletID == transaction.WalletID);

            if (wallet == null)
                return StatusCode(403, new { Message = "Ví không tồn tại!" });

            if (newTransaction.Amount < 1000)
                return BadRequest(new { Message = "Giao dịch phải lớn hơn hoặc bằng 1000 VNĐ" });

            var catagory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == transaction.CategoryID);

            if (catagory == null)
                return BadRequest(new { Message = "Danh mục không hợp lệ!" });

            if (catagory.Type == "Thu")
            {
                wallet.Balance -= transaction.Amount;
                wallet.Balance += newTransaction.Amount;
            }
            else if (catagory.Type == "Chi")
            {
                wallet.Balance += transaction.Amount;
                wallet.Balance -= newTransaction.Amount;    
            }
            transaction.Amount = newTransaction.Amount;
            transaction.Description = newTransaction.Description;
            transaction.TransactionDate = newTransaction.TransactionDate;

            _context.Transactions.Update(transaction);
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Sửa giao dịch thành công!" });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (UserId == null)
                return Unauthorized(new { Message = "Người dùng không hợp lệ!" });

            int userId = int.Parse(UserId);

            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
                return NotFound(new { Message = "Không tìm thấy giao dịch này!" });

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId && w.WalletID == transaction.WalletID);

            if (wallet == null)
                return StatusCode(403, new { Message = "Người dùng không có quyền xóa giao dịch này!" });

            var catagory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == transaction.CategoryID);

            if (catagory == null)
                return BadRequest(new { Message = "Danh mục không hợp lệ!" });

            if (catagory.Type == "Thu")
            {
                wallet.Balance -= transaction.Amount;
            }
            else if (catagory.Type == "Chi")
            {
                wallet.Balance += transaction.Amount;
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Xóa giao dịch thành công!" });
        }
    }
}
