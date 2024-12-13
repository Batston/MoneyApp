using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyApp.Server.Data;

namespace MoneyApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly DBContext _context;

        public WalletController(DBContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWallet(int id)
        {
            var result = await _context.Wallets.Where(w => w.UserId == id).ToListAsync();

            if (result == null)
                return NotFound("Không tìm thấy ví của người dùng này!");
                
            return Ok(result);
        }
    }
}
