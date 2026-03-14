using Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Fixture
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public AppDbContext DbContext { get; private set; } = null!;

        public async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(
                    "Server=localhost;Database=MyApp_Test;Trusted_Connection=True;")
                .Options;

            DbContext = new AppDbContext(options);

            await DbContext.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            await DbContext.Database.EnsureDeletedAsync();
            await DbContext.DisposeAsync();
        }
    }

}
