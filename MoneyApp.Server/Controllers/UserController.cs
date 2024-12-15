using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoneyApp.Server.Data;
using MoneyApp.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoneyApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly DBContext _context;

        public UserController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetIdByToken()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserId == null) 
                return Unauthorized(new { Message = "Chưa xác thực người dùng!" });

            return Ok(new { UserId = int.Parse(UserId) });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginResquest loginResquest)
        {
            //example:
            //
            //  {
            //      "username": "string",
            //      "password": "string"
            //  }
            
        var user = _context.Users.SingleOrDefault(u => u.Username == loginResquest.Username);

            if (user == null)
            {
                return Unauthorized( new { Message = "Invalid username" });
            }
            else if (VerifyPassword(loginResquest.Password, user.PasswordHash) == false)
            {
                return Unauthorized( new { Message = "Password is not correct!" });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("anthing_api_key_you_want_anthing_api_key_you_want_anthing_api_key_you_want"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost",
                audience: "moneyapp api",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            // userId luon luon 0
            //Example: 
            //
            //  {
            //      "userId": 0,
            //      "username": "string", 
            //      "passwordHash": "string",
            //      "fullName": "string",
            //      "email": "user@example.com", 
            //  }

            var isExist = _context.Users.SingleOrDefault(u => u.Username == newUser.Username);

            if (isExist != null)
                return BadRequest("Username has exist!");

            var PasswordHashed = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash);
            newUser.PasswordHash = PasswordHashed;
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return Ok("Sign up success!");
        }
    }
}
