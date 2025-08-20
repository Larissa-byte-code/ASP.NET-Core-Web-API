using Microsoft.AspNetCore.Mvc;
using ShooperAPI.Models;
using ShooperAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ShooperAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(User user)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (exists)
                return BadRequest("Cet email est déjà utilisé");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Inscription réussie !");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
                return BadRequest("Email ou mot de passe incorrect");

            return Ok($"Bienvenue {user.Name} !");
        }
        //Cette méthode permet à ton frontend (React, par exemple) de récupérer tous les utilisateurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers() => await _context.Users.ToListAsync();

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser) {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id) {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
