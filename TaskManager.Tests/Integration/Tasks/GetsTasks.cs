using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TaskManager.Tests.Integration.Tasks
{
    public class GetsTasks : IntegrationApiTestBase
    {
        [Fact]
        public async Task GetsAllTasks()
        {
            await SetupData();
            var response = await SendGetRequest("/api/tasks");
            var result = await GetJsonObjectArray<TaskResponse>(response);

            var expected = new List<TaskResponse>
            {
                new TaskResponse
                {
                    Name = "TaskOne",
                    Category = "CategoryOne"
                },
                new TaskResponse
                {
                    Name = "TaskTwo",
                    Category = "CategoryTwo"
                },
            };

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task FiltersByCategory()
        {
            await SetupData();
            var response = await SendGetRequest("/api/tasks?category=CategoryOne");
            var result = await GetJsonObjectArray<TaskResponse>(response);

            var expected = new List<TaskResponse>
            {
                new TaskResponse
                {
                    Name = "TaskOne",
                    Category = "CategoryOne"
                },
            };

            result.Should().BeEquivalentTo(expected);
        }

        private async Task SetupData()
        {
            var context = Server.CreateDbContext();
            await CreateTask("TaskOne", "CategoryOne", context);
            await CreateTask("TaskTwo", "CategoryTwo", context);
        }
    }
}