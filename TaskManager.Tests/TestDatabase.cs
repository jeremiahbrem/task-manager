using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TaskManager.Tests
{
    public sealed class TestDatabase
    {
        private readonly InMemoryDatabaseRoot _root = new ();
        private readonly DbContextOptions<Database.TaskManagerContext> _databaseOptions;
        public TestDatabase()
        {
            _databaseOptions = new DbContextOptionsBuilder<Database.TaskManagerContext>()
                .UseInMemoryDatabase("db", _root)
                .EnableServiceProviderCaching(cacheServiceProvider: false)
                .Options;
        }

        public Database.TaskManagerContext CreateDbContext()
        {
            return new (_databaseOptions);
        }
    }
}