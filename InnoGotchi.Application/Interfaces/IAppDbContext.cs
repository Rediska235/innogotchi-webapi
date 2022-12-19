using InnoGotchi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchi.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Farm> Farms { get; set; }
        DbSet<Pet> Pets { get; set; }
        DbSet<FriendFarm> FriendFarms { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
