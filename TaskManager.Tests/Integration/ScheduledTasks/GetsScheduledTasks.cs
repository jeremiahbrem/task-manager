using System;
using System.Collections.Generic;
using Task = System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TaskManager.Tests.Integration.ScheduledTasks
{
    public class GetsScheduledTasks : IntegrationApiTestBase
    {
        [Fact]
        public async Task.Task GetsAllScheduledTasks()
        {
            var context = Server.CreateDbContext();
            var user = await CreateUser("Jane", "Doe", "jane.doe@example.com", context);
            var precedingId = Guid.NewGuid().ToString();

            var precedingTask  = await CreateTask("Test Task", "Category One", context);
            var precedingScheduledTask = await CreateScheduledTask(precedingTask, user, context, null, precedingId);

            var task = await CreateTask("Test Task Two", "Category One", context);
            var id = Guid.NewGuid().ToString();
            await CreateScheduledTask(task, user, context, precedingScheduledTask, id);

            var response = await SendGetRequest($"/api/scheduled-tasks");
            var result = await GetJsonObjectArray<ScheduledTaskResponse>(response);

            var expected = new List<ScheduledTaskResponse>
            {
                new ScheduledTaskResponse
                {
                    User = user.ToQueryObject(),
                    Task = task.ToQueryObject(),
                    Preceding = precedingScheduledTask.Task.Name,
                    PrecedingId = precedingScheduledTask.ScheduledTaskId,
                    Id = id,
                },
                new ScheduledTaskResponse
                {
                    User = user.ToQueryObject(),
                    Task = precedingTask.ToQueryObject(),
                    Preceding = null,
                    PrecedingId = null,
                    Id = precedingId,
                },
            };

            result.Should().BeEquivalentTo(expected);
        }
    }
}