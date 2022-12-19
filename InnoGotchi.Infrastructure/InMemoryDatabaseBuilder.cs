using InnoGotchi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchi.Infrastructure
{
    public class InMemoryDatabaseBuilder
    {
        public static AppDbContext Build()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
