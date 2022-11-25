using InnoGotchi_WebApi.Models.UserModels;
using InnoGotchi_WebApi.Models.FarmModels;
using InnoGotchi_WebApi.Models.PetModels;
using Microsoft.EntityFrameworkCore;
using InnoGotchi_WebApi.Models;

namespace InnoGotchi_WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<FriendFarm> FriendFarms { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(b => b.Farm)
                .WithOne(i => i.User)
                .HasForeignKey<Farm>(b => b.UserId);
            
            modelBuilder.Entity<Farm>()
                .HasMany(c => c.Pets)
                .WithOne(e => e.Farm);
            
            modelBuilder.Entity<FriendFarm>()
                .HasKey(t => new { t.UserId, t.FarmId });

            modelBuilder.Entity<FriendFarm>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.FriendsFarms)
                .HasForeignKey(pt => pt.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
