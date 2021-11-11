using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Validation;
using Xunit;

namespace TaskManager.Tests.Integration.ScheduledTasks
{
    public class CreatesScheduledTasks : IntegrationApiTestBase
    {
        [Fact]
        public async Task CreateScheduledTaskTest()
        {
            var context = Server.CreateDbContext();

            var task  = await CreateTask("Test Task", "Category One", context);
            var user = await CreateUser("Jane", "Doe", "jane.doe@example.com", context);
            await CreateScheduledTask(task, user, context);

            var precedingScheduledTask = await context.ScheduledTasks.FirstOrDefaultAsync();

            var newTask  = await CreateTask("Test Task Two", "Category One", context);

            var scheduledTask = new
            {
                Task = newTask.Name,
                PrecedingId = precedingScheduledTask.ScheduledTaskId,
                Email = user.Email
            };

            var content = CreateContent(scheduledTask);

            var response = await SendPostRequest("/api/scheduled-tasks/create", content);

            var createdScheduledTask = await context.ScheduledTasks.FirstOrDefaultAsync(x => x.Task.Name == newTask.Name);
            var createdTask = await context.Tasks.FirstOrDefaultAsync(x => x.Name == newTask.Name);
            var createdUser = await context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            createdScheduledTask.Should().BeEquivalentTo(new
            {
                Task = createdTask,
                User = createdUser,
                PrecedingTask = precedingScheduledTask
            });
        }

        [Fact]
        public async Task SendsNonExistentTaskError()
        {
            var context = Server.CreateDbContext();

            var user = await CreateUser("Jane", "Doe", "jane.doe@example.com", context);

            var scheduledTask = new
            {
                Task = "unknownTask",
                Email = user.Email
            };

            var expected = CreateExpectedResponse(
                "Invalid task",
                "Invalid task unknownTask. You must use an existing task."
            );

            await AssertResponse(expected, scheduledTask);
        }

        [Fact]
        public async Task SendsNonExistentUserError()
        {
            var context = Server.CreateDbContext();

            var task  = await CreateTask("Test Task", "Category One", context);

            var scheduledTask = new
            {
                Task = task.Name,
                Email = "unknown@example.com"
            };

            var expected = CreateExpectedResponse(
                "Invalid user",
                "A user with email unknown@example.com does not exist."
            );

            await AssertResponse(expected, scheduledTask);
        }

        [Fact]
        public async Task SendsNonExistentPrecedingError()
        {
            var context = Server.CreateDbContext();

            var task  = await CreateTask("Test Task", "Category One", context);
            var user = await CreateUser("Jane", "Doe", "jane.doe@example.com", context);

            var scheduledTask = new
            {
                Task = task.Name,
                Email = user.Email,
                PrecedingId = "abc123"
            };

            var expected = CreateExpectedResponse(
                "Invalid preceding id",
                "A scheduled task with id abc123 does not exist."
            );

            await AssertResponse(expected, scheduledTask);
        }

        private async Task AssertResponse(ValidationResponse expected, object scheduledTask)
        {
            var content = CreateContent(scheduledTask);

            var response = await SendPostRequest("/api/scheduled-tasks/create", content);
            var result = await GetJsonObject<ValidationResponse>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }
    }
}