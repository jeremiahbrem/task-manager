using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace TaskManager.Tests.Integration.Categories
{
    public class GetsCategories : IntegrationApiTestBase
    {
        [Fact]
        public async Task GetsAllCategories()
        {
            var context = Server.CreateDbContext();
            await CreateCategory("CategoryOne", context);
            await CreateCategory("CategoryTwo", context);

            var response = await SendGetRequest("/api/categories");

            var result = await GetJArray(response);

            var expected = new JArray("CategoryOne", "CategoryTwo");
            result.Should().BeEquivalentTo(expected);
        }
    }
}