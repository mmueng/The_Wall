using Microsoft.EntityFrameworkCore;

namespace The_Wall.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Comments> Comments { get; set; }
        // public DbSet<Association> association { get; set; }
    }
}