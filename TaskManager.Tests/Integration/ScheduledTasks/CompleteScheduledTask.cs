using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Validation;
using TaskManager.Database;
using TaskManager.Models.Domain.ScheduledTask;
using TaskDomain = TaskManager.Models.Domain.Task;
using Xunit;

namespace TaskManager.Tests.Integration.ScheduledTasks
{
    public class CompleteScheduledTask : IntegrationApiTestBase
    {
        [Fact]
        public async Task CompleteTaskTest()
        {
            var ctx = Server.CreateDbContext();
            var scheduledTask = await SetupData(ctx);
            var id = scheduledTask.ScheduledTaskId;
            var route = $"/api/scheduled-tasks/complete/{id}";
            var email = scheduledTask.User.Email;

            var content = CreateContent(new { });

            scheduledTask.PrecedingTask!.Complete();
            ctx.Update(scheduledTask);
            await ctx.SaveChangesAsync();

            var response = await SendPostRequest(route, content, email);
            var result = await GetJsonObject<ValidationResponse>(response);
            var expected = CreateExpectedResponse($"Scheduled task {id} completed.");

            await ctx.Entry(scheduledTask).ReloadAsync();
            var updated = await ctx.ScheduledTasks.FirstAsync(x => x.ScheduledTaskId == id);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(updated.Completed);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ReturnsBadRequestIfPrecedingNotCompleted()
        {
            var ctx = Server.CreateDbContext();
            var scheduledTask = await SetupData(ctx);
            var id = scheduledTask.ScheduledTaskId;
            var route = $"/api/scheduled-tasks/complete/{id}";
            var email = scheduledTask.User.Email;

            var content = CreateContent(new { });

            var response = await SendPostRequest(route, content, email);
            var result = await GetJsonObject<ValidationResponse>(response);
            var expected = CreateExpectedResponse("Complete scheduled task error", "You must complete the preceding task first");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        private async Task<ScheduledTask> SetupData(TaskManagerContext context)
        {
            var user = await CreateUser("Jane", "Doe", "jane.doe@example.com", context);

            var precedingTask  = await CreateTask("Test Task", "Category One", context);
            var precedingScheduledTask = await CreateScheduledTask(precedingTask, user, context);

            var task = await CreateTask("Test Task Two", "Category One", context);
            var id = Guid.NewGuid().ToString();
            return await CreateScheduledTask(task, user, context, precedingScheduledTask, id);
        }
    }
}