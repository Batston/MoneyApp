using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MoneyApp.Server.Data;

namespace MoneyApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly DBContext _context;
        public CategoryController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized(new { Message = "Người dùng không hợp lệ!" });

            var category = await _context.Categories.ToListAsync();

            if (category == null)
                return NotFound(new { Message = "Không tìm thấy danh mục!" });

            return Ok(category);
        }
    }
}