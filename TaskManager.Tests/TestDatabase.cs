using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TaskManager.Models;
using TaskManager.Models.Categories;
using TaskManager.Models.Task;
using TaskManager.Models.User;

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

            using var ctx = CreateDbContext();

            ctx.Database.EnsureCreated();
            ctx.Categories.Add(new Category { Name = "CategoryOne"});
            ctx.Categories.Add(new Category { Name = "CategoryTwo"});
            ctx.Users.Add(new User {FirstName = "John", LastName = "Doe", Email = "john.doe@example.com"});
            ctx.SaveChanges();

            var category = ctx.Set<Category>().First();

            ctx.Tasks.Add(new Task
            {
                Name = "TaskOne",
                Category = category,
            });
            ctx.SaveChanges();
        }

        public Database.TaskManagerContext CreateDbContext()
        {
            return new (_databaseOptions);
        }
    }
}