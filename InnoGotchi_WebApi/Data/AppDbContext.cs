using InnoGotchi_WebApi.Models.User;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchi_WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
