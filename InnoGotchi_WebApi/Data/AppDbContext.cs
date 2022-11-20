using InnoGotchi_WebApi.Models.User;
using InnoGotchi_WebApi.Models.Farm;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchi_WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Farm> Farms { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
