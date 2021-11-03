using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using TaskManager.Tests.Mamas;
using Xunit;

namespace TaskManager.Tests.Integration.Tasks
{
    public class GetTask : TaskTestBase
    {
        [Fact]
        public async Task GetsTask()
        {
            await CreateTask("TaskOne", "CategoryOne");

            var response = await SendGetRequest("/api/tasks/TaskOne");
            var result = await GetJsonObject<TaskResponse>(response);

            var expected = new TaskResponse
            {
                Name = "TaskOne",
                Category = "CategoryOne"
            };

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ReturnsNotFound()
        {
            var response = await SendGetRequest("/api/tasks/unknownTask");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}