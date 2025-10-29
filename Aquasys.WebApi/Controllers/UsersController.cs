using Aquasys.Core.Entities;
using Aquasys.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aquasys.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/{globalId}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.GlobalId == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            // Garante que a data seja UTC e gera um novo GlobalId se necessário
            user.LastModifiedAt = DateTime.UtcNow;
            if (user.GlobalId == Guid.Empty)
            {
                user.GlobalId = Guid.NewGuid();
            }

            // Validação simples (pode ser aprimorada com FluentValidation, etc.)
            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Nome de usuário e senha são obrigatórios.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Retorna 201 Created com a localização do novo recurso e o objeto criado
            return CreatedAtAction(nameof(GetUser), new { id = user.GlobalId }, user);
        }

        // PUT: api/Users/{globalId}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.GlobalId)
            {
                return BadRequest("O GlobalId na URL não corresponde ao GlobalId no corpo da requisição.");
            }

            // Garante que a data de modificação seja atualizada
            user.LastModifiedAt = DateTime.UtcNow;

            _context.Entry(user).State = EntityState.Modified;

            // Evita que a chave primária numérica seja alterada (se aplicável, embora User não tenha ID numérico visível aqui)
            // _context.Entry(user).Property(u => u.IDUser).IsModified = false; 

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Re-lança a exceção se for outro erro de concorrência
                }
            }

            return NoContent(); // Retorna 204 No Content (sucesso, sem corpo de resposta)
        }

        // DELETE: api/Users/{globalId}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.GlobalId == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna 204 No Content
        }

        // Método auxiliar para verificar se um usuário existe
        private async Task<bool> UserExists(Guid id)
        {
            return await _context.Users.AnyAsync(e => e.GlobalId == id);
        }
    }
}