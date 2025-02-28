using Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options) { }
        
    
    public DbSet<User> Users { get; set; }

    }
}