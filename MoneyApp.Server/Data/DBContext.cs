using Microsoft.EntityFrameworkCore;
using MoneyApp.Server.Models;

namespace MoneyApp.Server.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }
        public DbSet<User> Users { get; set; } 
    }
}
