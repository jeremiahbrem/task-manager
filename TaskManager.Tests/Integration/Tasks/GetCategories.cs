using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace TaskManager.Tests.Integration.Tasks
{
    public class GetTasks  : IntegrationApiTestBase
    {
        [Fact (Skip ="not built")]
        public async Task GetsAllTasks()
        {
            // var response = await SendGetRequest("/api/categories");
            //
            // var result = await GetJsonObject(response);
            //
            // var expected = new JArray("CategoryOne", "CategoryTwo");
            // result.Should().BeEquivalentTo(expected);
        }
    }
}