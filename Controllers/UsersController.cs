using Microsoft.AspNetCore.Mvc;
using ShooperAPI.Models;
using ShooperAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ShooperAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔐 Inscription avec hash du mot de passe
        [HttpPost("signup")]
        public async Task<IActionResult> Signup(User user)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (exists)
                return BadRequest("Cet email est déjà utilisé");

            // Hash du mot de passe
            user.Password = _hasher.HashPassword(user, user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Inscription réussie !");
        }

        // 🔐 Connexion avec vérification du hash
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return BadRequest("Email ou mot de passe incorrect");

            var result = _hasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (result != PasswordVerificationResult.Success)
                return BadRequest("Email ou mot de passe incorrect");

            return Ok($"Bienvenue {user.Name} !");
        }

        // 📋 Récupérer tous les utilisateurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers() => await _context.Users.ToListAsync();

        // ✏️ Modifier un utilisateur
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // 🗑️ Supprimer un utilisateur
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
