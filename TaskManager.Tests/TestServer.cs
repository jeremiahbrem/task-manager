using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskManager.Database;

namespace TaskManager.Tests
{
    public sealed class TestServer : WebApplicationFactory<Startup>
    {
        private readonly TestDatabase _db = new ();

        public TestServer()
        {
            CreateClient();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices((services) =>
            {
                services.AddLogging(cfg =>
                {
                    cfg.ClearProviders();
                });

                services.AddSingleton(this);

                var toRemove = new[]
                    {
                        typeof(TaskManagerContext),
                        typeof(DbContextOptions)
                    }.SelectMany(x => services.Where(svc => svc.ServiceType == x))
                    .ToList();
                toRemove.ForEach(x => services.Remove(x));

                services.AddScoped(ctx => _db.CreateDbContext());
            });
        }
        public TaskManagerContext CreateDbContext()
        {
            return _db.CreateDbContext();
        }

        public HttpClient CreateHttpClient()
        {
            return CreateClient();
        }
    }
}