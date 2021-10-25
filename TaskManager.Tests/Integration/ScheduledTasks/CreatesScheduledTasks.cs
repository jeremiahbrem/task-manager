using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskManager.Common.Validation;
using Xunit;

namespace TaskManager.Tests.Integration.ScheduledTasks
{
    public class CreatesScheduledTasks : IntegrationApiTestBase
    {
        [Fact]
        public async Task CreateScheduledTaskTest()
        {

            var scheduledTask = new
            {
                task = new { name = "Test Task", category = "CategoryOne" },
                preceding = new
                {
                    task = new { name = "Task 2", category = "CategoryTwo" },
                    user = new
                    {
                        firstName = "Jane",
                        lastName = "Doe",
                        email = "jane.doe@example.com",
                    }
                }
            };

            var content = CreateContent(scheduledTask);

            var response = await SendPostRequest("/api/scheduled-tasks/create", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(Skip = "setup")]
        public async Task SendsDuplicateTaskError()
        {
            var task = new
            {
                name = "TaskOne",
                category = "CategoryOne",
            };

            var content = CreateContent(task);

            var response = await SendPostRequest("/api/tasks/create", content);
            var result = await GetJsonObject<ValidationResponse>(response);

            var expected = CreateExpectedResponse(
                "Duplicate task error",
                "A task with name TaskOne already exists."
            );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact(Skip = "setup")]
        public async Task SendsInvalidCategoryError()
        {
            var task = new
            {
                name = "TaskOne",
                category = "NonExistentCategory",
            };

            var content = CreateContent(task);

            var response = await SendPostRequest("/api/tasks/create", content);
            var result = await GetJsonObject<ValidationResponse>(response);

            var expected = CreateExpectedResponse(
                "Invalid category",
                "Invalid category NonExistentCategory. You must use an existing category."
            );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact(Skip = "setup")]
        public async Task SendsInvalidModelError()
        {
            var task = new
            {
                someKey = "Some property",
                name = "new task"
            };

            var content = CreateContent(task);
            var response = await SendPostRequest("/api/tasks/create", content);

            var result = await GetJsonObject<ValidationResponse>(response);

            var expected = CreateExpectedResponse(
                "Validation failed",
                "The Category field is required."
            );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }
    }
}