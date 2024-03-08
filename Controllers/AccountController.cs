using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PairExpensesAPI.Data;
using PairXpensesAPI;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PairExpensesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;


        public AccountController(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context; 
        }

        public class LoginModel
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid request payload");
            }
            else
            {
                var user = Authenticate(model.Username, model.Password);
                if (user is not null)
                {
                    var token = GenerateJwt(user);
                    return Ok(token);
                }
            }           

            

            return NotFound("User not found");
        }


        private User? Authenticate(string Username, string Password)
        {
            var currentUser = _context.Users
                .FirstOrDefault(user => user.Username.ToLower() == Username.ToLower()
                   && user.Password == Password);

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }
        private string GenerateJwt(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Crear los claims
            var claims = new[]
            {
             new Claim(JwtClaimTypes.Subject, user.Username),
             new Claim(JwtClaimTypes.Name, user.Name),
             new Claim(JwtClaimTypes.Role, user.PairRole),
         };


            // Crear el token

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
