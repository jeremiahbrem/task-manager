using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using TaskManager.Common.Validation;
using TaskManager.Models.Domain.ScheduledTask;
using TaskDomain = TaskManager.Models.Domain.Task;
using Xunit;

namespace TaskManager.Tests.Integration.ScheduledTasks
{
    public class GetsScheduledTask : IntegrationApiTestBase
    {
        [Fact]
        public async Task GetsScheduledTaskTest()
        {
            var scheduledTask = await SetupData();

            var expected = new ScheduledTaskResponse
            {
                User = scheduledTask.User.ToQueryObject(),
                Task = scheduledTask.Task.ToQueryObject(),
                Preceding = scheduledTask.PrecedingTask!.Task.Name,
                PrecedingId = scheduledTask.PrecedingTask.ScheduledTaskId,
                Id = scheduledTask.ScheduledTaskId,
                Completed = false,
            };

            var response = await SendGetRequest($"/api/scheduled-tasks/{expected.Id}", expected.User.Email);
            var result = await GetJsonObject<ScheduledTaskResponse>(response);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ReturnsBadRequestWithEmptyEmail()
        {
            var scheduledTask = await SetupData();
            var expected = CreateExpectedResponse("Missing email", "You must include an email address");
            var id = scheduledTask.ScheduledTaskId;

            var response = await SendGetRequest($"/api/scheduled-tasks/{id}");
            var result = await GetJsonObject<ValidationResponse>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ReturnsUnauthorizedWithUnknownEmail()
        {
            var scheduledTask = await SetupData();
            var expected = CreateExpectedResponse("Unauthorized", "A user with email unknown@example.com does not exist");
            var id = scheduledTask.ScheduledTaskId;

            var response = await SendGetRequest($"/api/scheduled-tasks/{id}", "unknown@example.com");
            var result = await GetJsonObject<ValidationResponse>(response);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ReturnsUnauthorized()
        {
            var scheduledTask = await SetupData();
            var id = scheduledTask.ScheduledTaskId;
            await CreateUser("John", "Doe", "other@example.com", Server.CreateDbContext());

            var expected = CreateExpectedResponse("Unauthorized", "You are not authorized to access this scheduled task.");

            var response = await SendGetRequest($"/api/scheduled-tasks/{id}", "other@example.com");
            var result = await GetJsonObject<ValidationResponse>(response);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task NotFound()
        {
            var scheduledTask = await SetupData();
            var email = scheduledTask.User.Email;
            var expected = CreateExpectedResponse("Invalid id", "A scheduled task with id unknownId does not exist.");

            var response = await SendGetRequest("/api/scheduled-tasks/unknownId", email);
            var result = await GetJsonObject<ValidationResponse>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        private async Task<ScheduledTask> SetupData()
        {
            var context = Server.CreateDbContext();
            var user = await CreateUser("Jane", "Doe", "jane.doe@example.com", context);

            var precedingTask  = await CreateTask("Test Task", "Category One", context);
            var precedingScheduledTask = await CreateScheduledTask(precedingTask, user, context);

            var task = await CreateTask("Test Task Two", "Category One", context);
            var id = Guid.NewGuid().ToString();
            return await CreateScheduledTask(task, user, context, precedingScheduledTask, id);
        }
    }
}