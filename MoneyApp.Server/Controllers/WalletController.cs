using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyApp.Server.Data;
using MoneyApp.Server.Models;
using System.Security.Claims;

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
        [Authorize]
        public async Task<IActionResult> GetWallet(int id)
        {
            var result = await _context.Wallets.Where(w => w.UserId == id).ToListAsync();

            if (result == null)
                return NotFound("Không tìm thấy ví của người dùng này!");
                
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddWallet([FromBody] Wallet newWallet)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized(new {Message = "Người dùng không hợp lệ!"});

            if (newWallet.UserId != int.Parse(userId))
                return StatusCode(403, new { Message = "Người dùng không có quyền thêm ví!" });

            _context.Wallets.Add(newWallet);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Thêm ví thành công!"});
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Nguời dùng không hợp lệ!");

            var wallet = await _context.Wallets.FindAsync(id);

            if (wallet == null)
                return NotFound("Không tìm thấy ví của người dùng này");

            if (wallet.UserId != int.Parse(userId))
                return StatusCode(403 ,new {Message = "Người dùng không có quyền thực hiện yều cầu này!"});

            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Xóa ví thành công!" }); 
        }
    }
}
