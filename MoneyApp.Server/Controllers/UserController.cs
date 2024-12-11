using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoneyApp.Server.Data;
using MoneyApp.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

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

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginResquest loginResquest)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == loginResquest.Username);

            if (user == null && VerifyPassword(loginResquest.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password");
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("anthing_api_key_you_want_anthing_api_key_you_want_anthing_api_key_you_want_anthing_api_key_you_want"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost",
                audience: "moneyapp Api",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            // Hash password nhập vào và so sánh với hash lưu trong DB
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
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
