using Aquasys.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aquasys.WebApi.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Usuário e senha são obrigatórios.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == loginRequest.Username && u.Password == loginRequest.Password);

            if (user == null)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            // Em um app real, aqui retornaríamos um Token JWT.
            // Por simplicidade, vamos retornar o próprio objeto do usuário.
            return Ok(user);
        }
    }
}
