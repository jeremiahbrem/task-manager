using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TaskManager.Tests.Integration.Tasks
{
    public class CreateTasks : IntegrationApiTestBase
    {
        [Fact]
        public async Task CreateTaskTest()
        {
            var task = new
            {
                name = "Test Task",
                category = "CategoryOne",
            };

            var content = CreateContent(task);

            var response = await SendPostRequest("/api/tasks/create", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendsDuplicateTaskError()
        {
            var task = new
            {
                name = "TaskOne",
                category = "CategoryOne",
            };

            var content = CreateContent(task);

            var response = await SendPostRequest("/api/tasks/create", content);
            var result = await GetJsonObject(response);

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
                name = "TaskOne",
                category = "NonExistentCategory",
            };

            var content = CreateContent(task);

            var response = await SendPostRequest("/api/tasks/create", content);
            var result = await GetJsonObject(response);

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

            var result = await GetJsonObject(response);

            var expected = CreateExpectedResponse(
                "Validation failed",
                "The Category field is required."
            );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }
    }
}