using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace TaskManager.Tests.Integration.Tasks
{
    public class GetsTasks : IntegrationApiTestBase
    {
        [Fact]
        public async Task GetsAllTasks()
        {
            var response = await SendGetRequest("/api/tasks");
            var result = await GetJArray(response);

            var expected = new List<JObject>
            {
                new (
                    new JProperty("name", "TaskOne"),
                    new JProperty("category", "CategoryOne")
                )
            };

            result.Should().BeEquivalentTo(expected);
        }
    }
}