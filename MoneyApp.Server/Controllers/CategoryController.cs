using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MoneyApp.Server.Data;
using MoneyApp.Server.Models;

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

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized(new { Message = "Người dùng không hợp lệ!" });

            var category = new List<Category> { };

            if (id == 0) //all
            {
                category = await _context.Categories.ToListAsync();

                if (category == null)
                    return NotFound(new { Message = "Không tìm thấy danh mục!" });
            }
            else if (id == 1) //chi
            {
                category = await _context.Categories.Where(c => c.Type == "Chi").ToListAsync();

                if (category == null)
                    return NotFound(new { Message = "Không tìm thấy danh mục chi!" });
            }
            else if (id == 2) //thu
            {
                category = await _context.Categories.Where(c => c.Type == "Thu").ToListAsync();

                if (category == null)
                    return NotFound(new { Message = "Không tìm thấy danh mục Thu!" });
            }

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory( )
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