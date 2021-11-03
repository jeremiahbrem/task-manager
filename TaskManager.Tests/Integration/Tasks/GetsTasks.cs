using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace TaskManager.Tests.Integration.Tasks
{
    public class GetsTasks : TaskTestBase
    {
        [Fact]
        public async Task GetsAllTasks()
        {
            await CreateTask("TaskOne", "CategoryOne");
            await CreateTask("TaskTwo", "CategoryTwo");
            var response = await SendGetRequest("/api/tasks");
            var result = await GetJArray(response);

            var expected = new List<JObject>
            {
                new (
                    new JProperty("name", "TaskOne"),
                    new JProperty("category", "CategoryOne")
                ),
                new (
                    new JProperty("name", "TaskTwo"),
                    new JProperty("category", "CategoryTwo")
                ),
            };

            result.Should().BeEquivalentTo(expected);
        }
    }
}