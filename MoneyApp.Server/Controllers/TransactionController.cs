using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyApp.Server.Data;
using MoneyApp.Server.Models;
using System.Transactions;

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
    }
}
