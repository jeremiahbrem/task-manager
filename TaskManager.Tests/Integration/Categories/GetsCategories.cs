using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using TaskManager.Tests.Mamas;
using Xunit;

namespace TaskManager.Tests.Integration.Categories
{
    public class GetsCategories : IntegrationApiTestBase
    {
        [Fact]
        public async Task GetsAllCategories()
        {
            var motherOne = new CategoryMother("CategoryOne");
            var motherTwo = new CategoryMother("CategoryTwo");
            var context = Server.CreateDbContext();
            context.Categories.Add(motherOne.Category);
            context.Categories.Add(motherTwo.Category);
            await context.SaveChangesAsync();

            var response = await SendGetRequest("/api/categories");

            var result = await GetJArray(response);

            var expected = new JArray("CategoryOne", "CategoryTwo");
            result.Should().BeEquivalentTo(expected);
        }
    }
}