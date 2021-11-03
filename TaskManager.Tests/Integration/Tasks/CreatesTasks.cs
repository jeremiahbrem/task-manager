using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using TaskManager.Common.Validation;
using TaskManager.Tests.Mamas;
using Xunit;

namespace TaskManager.Tests.Integration.Tasks
{
    public class CreatesTasks : TaskTestBase
    {
        [Fact]
        public async Task CreateTaskTest()
        {
            var context = Server.CreateDbContext();
            context.Categories.Add(new CategoryMother("Category One").Category);
            await context.SaveChangesAsync();

            var task = new
            {
                Name = "Task One",
                Category = "Category One"
            };

            var content = CreateContent(task);

            var response = await SendPostRequest("/api/tasks/create", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendsDuplicateTaskError()
        {
            await CreateTask("TaskOne", "CategoryOne");

            var duplicate = new
            {
                Name = "TaskOne",
                Category = "CategoryOne"
            };

            var content = CreateContent(duplicate);

            var response = await SendPostRequest("/api/tasks/create", content);
            var result = await GetJsonObject<ValidationResponse>(response);

            var expected = CreateExpectedResponse(
                "Duplicate task error",
                "A task with name TaskOne already exists."
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
                "Invalid category",
                "Invalid category NonExistentCategory. You must use an existing category."
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