using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;

namespace MoneyApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost(Name = "LOGIN")]
        [Route("login")]
        public async Task<IActionResult> UserCheckAuthentication()
        {
            try
            {
                if (Request.ContentLength == null || Request.ContentLength == 0)
                {
                    return BadRequest(new { message = "Please input Username and Password!" });
                }

                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                if (connection.State == ConnectionState.Closed)
                {
                    await connection.OpenAsync();
                }

                using SqlCommand command = new();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spAdmin_UserLogin";

                using (var reader = new StreamReader(Request.Body))
                {
                    var requestBody = await reader.ReadToEndAsync();

                    // Parse the JSON content to a JObject
                    var jsonObject = JObject.Parse(requestBody);
                    if (jsonObject["UserName"] == null)
                        return BadRequest(new { message = "Please input Username!" });

                    command.Parameters.AddWithValue("@UserName", jsonObject["UserName"]?.Value<string>());
                    command.Parameters.AddWithValue("@PassWord", jsonObject["PassWord"]?.Value<string>());
                }

                SqlDataAdapter da = new(command);
                DataTable dt = new();
                da.Fill(dt);

                var jwt = new JwtToken();

                var tokenString = jwt.GenerateJwtToken(
                  username: dt.Rows[0]["UserName"].ToString()
                );

                return Ok(new { token = tokenString });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
        }
    }
}
