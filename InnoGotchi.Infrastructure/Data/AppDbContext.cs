using InnoGotchi.Application.Interfaces;
using InnoGotchi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace InnoGotchi.Infrastructure.Data
{ 
    public class AppDbContext : DbContext, IAppDbContext
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
            modelBuilder.Entity<Pet>(ConfigurePet);
            modelBuilder.Entity<Farm>(ConfigureFarm);
            modelBuilder.Entity<User>(ConfigureUser);
            modelBuilder.Entity<FriendFarm>(ConfigureFriendFarm);
        }

        private void ConfigurePet(EntityTypeBuilder<Pet> builder)
        {            
            builder.HasOne(x => x.Farm)
                .WithMany(x => x.Pets)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private void ConfigureFarm(EntityTypeBuilder<Farm> builder)
        {
            builder.HasMany(x => x.Pets)
                .WithOne(x => x.Farm);
        }

        private void ConfigureUser(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(x => x.Farm)
                .WithOne(x => x.User)
                .HasForeignKey<Farm>(x => x.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
        
        private void ConfigureFriendFarm(EntityTypeBuilder<FriendFarm> builder)
        {
            builder.HasKey(x => new { x.UserId, x.FarmId });
            
            builder.HasOne(x => x.User)
                .WithMany(x => x.FriendsFarms)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
