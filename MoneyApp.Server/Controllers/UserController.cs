using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyApp.Server.Data;

namespace MoneyApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly DBContext _context;

        public UserController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUSer()
        {
            var result = _context.Users.ToList();
            return Ok(result);
        }
    }
}
