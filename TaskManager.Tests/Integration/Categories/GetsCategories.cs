using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace TaskManager.Tests.Integration.Categories
{
    public class GetsCategories : CategoryTestBase
    {
        [Fact]
        public async Task GetsAllCategories()
        {
            await CreateCategory("CategoryOne");
            await CreateCategory("CategoryTwo");

            var response = await SendGetRequest("/api/categories");

            var result = await GetJArray(response);

            var expected = new JArray("CategoryOne", "CategoryTwo");
            result.Should().BeEquivalentTo(expected);
        }
    }
}