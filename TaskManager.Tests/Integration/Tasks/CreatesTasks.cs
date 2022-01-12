using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Validation;
using Xunit;

namespace TaskManager.Tests.Integration.Tasks
{
    public class CreatesTasks : IntegrationApiTestBase
    {
        [Fact]
        public async Task CreateTaskTest()
        {
            var context = Server.CreateDbContext();
            await CreateCategory("Category One", context);

            var task = new
            {
                Name = "Task One",
                Category = "Category One"
            };

            var content = CreateContent(task);

            var response = await SendPostRequest("/api/tasks/create", content);

            var newTask = await context.Tasks.FirstOrDefaultAsync();

            newTask.Should().BeEquivalentTo(new { Name = "Task One", Category = new { Name = "Category One" } });
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendsDuplicateTaskError()
        {
            var context = Server.CreateDbContext();
            await CreateTask("TaskOne", "CategoryOne", context);

            var duplicate = new
            {
                Name = "TaskOne",
                Category = "CategoryOne"
            };

            var content = CreateContent(duplicate);

            var response = await SendPostRequest("/api/tasks/create", content);
            var result = await GetJsonObject<ValidationResponse>(response);

            var expected = CreateExpectedResponse(
                "Invalid task name",
                "A task with name TaskOne already exists"
            );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SendsInvalidCategoryError()
        {
            var task = new
            {
                Name = "Test Task",
                Category = "NonExistentCategory"
            };

            var content = CreateContent(task);

            var response = await SendPostRequest("/api/tasks/create", content);
            var result = await GetJsonObject<ValidationResponse>(response);

            var expected = CreateExpectedResponse(
                "Invalid category name",
                "A category with name NonExistentCategory was not found"
            );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
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