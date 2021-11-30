using System;
using System.Net;
using Task = System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TaskManager.Tests.Integration.ScheduledTasks
{
    public class GetsScheduledTask : IntegrationApiTestBase
    {
        [Fact]
        public async Task.Task GetsScheduledTaskTest()
        {
            var context = Server.CreateDbContext();
            var user = await CreateUser("Jane", "Doe", "jane.doe@example.com", context);

            var precedingTask  = await CreateTask("Test Task", "Category One", context);
            var precedingScheduledTask = await CreateScheduledTask(precedingTask, user, context);

            var task = await CreateTask("Test Task Two", "Category One", context);
            var id = Guid.NewGuid().ToString();
            await CreateScheduledTask(task, user, context, precedingScheduledTask, id);

            var response = await SendGetRequest($"/api/scheduled-tasks/{id}");
            var result = await GetJsonObject<ScheduledTaskResponse>(response);

            var expected = new ScheduledTaskResponse
            {
                User = user.ToQueryObject(),
                Task = task.ToQueryObject(),
                Preceding = precedingScheduledTask.Task.Name,
                PrecedingId = precedingScheduledTask.ScheduledTaskId,
                Id = id,
            };

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task.Task ReturnsNotFound()
        {
            var response = await SendGetRequest("/api/scheduled-tasks/unknownTask");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}