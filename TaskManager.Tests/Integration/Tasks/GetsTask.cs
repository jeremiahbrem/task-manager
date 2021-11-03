using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TaskManager.Tests.Integration.Tasks
{
    public class GetTask : IntegrationApiTestBase
    {
        [Fact]
        public async Task GetsTask()
        {
            var context = Server.CreateDbContext();
            await CreateTask("TaskOne", "CategoryOne", context);

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