using System;
using System.Net;
using System.Net.Http;
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
            var id = Guid.NewGuid().ToString();
            var precedingScheduledTask = await CreateScheduledTask(task, user, context, null, id);

            var newTask  = await CreateTask("Test Task Two", "Category One", context);
            var scheduledTask = new
            {
                Task = newTask.Name,
                PrecedingId = id,
            };

            var content = CreateContent(scheduledTask);

            var response = await SendPostRequest("/api/scheduled-tasks/create", content, user.Email);

            var createdScheduledTask = await context.ScheduledTasks.FirstOrDefaultAsync(x => x.Task.Name == newTask.Name);
            var createdTask = await context.Tasks.FirstOrDefaultAsync(x => x.Name == newTask.Name);
            var createdUser = await context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            createdScheduledTask.Should().BeEquivalentTo(new
            {
                Task = createdTask,
                User = createdUser,
                PrecedingTask = precedingScheduledTask,
                Completed = false
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
            };

            var expected = CreateExpectedResponse(
                "Invalid task",
                "Invalid task unknownTask. You must use an existing task."
            );

            var (response, result) = await GetResponse(scheduledTask, user.Email);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SendsNonExistentUserError()
        {
            var context = Server.CreateDbContext();

            var task  = await CreateTask("Test Task", "Category One", context);
            var uknownEmail = "unknown@example.com";

            var scheduledTask = new
            {
                Task = task.Name,
            };

            var expected = CreateExpectedResponse(
                "Unauthorized",
                "A user with email unknown@example.com does not exist"
            );

            var (response, result) = await GetResponse(scheduledTask, uknownEmail);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
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
                PrecedingId = "abc123"
            };

            var expected = CreateExpectedResponse(
                "Invalid preceding id",
                "A scheduled task with id abc123 does not exist."
            );

            var (response, result) = await GetResponse(scheduledTask, user.Email);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SendsInvalidModelError()
        {
            var context = Server.CreateDbContext();
            var user = await CreateUser("Jane", "Doe", "jane.doe@example.com", context);
            await CreateTask("Test Task", "Category One", context);

            var scheduledTask = new
            {
                PrecedingId = "abc123"
            };

            var expected = CreateExpectedResponse(
                "Validation failed",
                "The Task field is required."
            );

            var (response, result) = await GetResponse(scheduledTask, user.Email);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        private async Task<(HttpResponseMessage, ValidationResponse)> GetResponse(object scheduledTask, string email = null)
        {
            var content = CreateContent(scheduledTask);

            var response = await SendPostRequest("/api/scheduled-tasks/create", content, email);
            var result = await GetJsonObject<ValidationResponse>(response);
            return (response, result);
        }
    }
}