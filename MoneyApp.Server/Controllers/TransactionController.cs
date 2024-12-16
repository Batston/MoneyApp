using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyApp.Server.Data;
using MoneyApp.Server.Models;
using System.Security.Claims;
using System.Transactions;
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

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTransactions(int id)
        {
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

            if(userId == null)
                return Unauthorized( new { Message = "Người dùng không hợp lệ!"});

            int id = int.Parse(userId);

            var isWalletExist = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == id && w.WalletID == transaction.WalletID);

            if (isWalletExist == null)
                return NotFound(new { Message = "Người dùng không được thực hiện giao dịch trên ví này!" });

            var isCategoryCorrect = await _context.Categories.FindAsync(transaction.CategoryID);

            if (isCategoryCorrect == null)
                return BadRequest(new { Message = "Danh mục không hợp lệ!" });

            if (isWalletExist.Balance < transaction.Amount)
                return BadRequest(new { Message = "Số  dư ví không đủ để thực hiện giao dịch!" });
            else 
                isWalletExist.Balance -= transaction.Amount;

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return Ok( new { Message = "Thêm giao  dịch thành công!"});
        }
    }
}
