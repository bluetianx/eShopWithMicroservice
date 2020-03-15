using Identity.Entity;
using Microsoft.EntityFrameworkCore;
namespace Identity.Context
{
    public class IdentityDbContext:DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<User> Users { get; set; }
    }
}