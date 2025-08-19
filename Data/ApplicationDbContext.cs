using Microsoft.EntityFrameworkCore;
using ShooperAPI.Models;

namespace ShooperAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {}

        public DbSet<User> Users { get; set; }
    }
}
