using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using TaskManager.Common.Validation;
using Xunit;

namespace TaskManager.Tests.Integration.Users
{
    public class CreatesUsers : IntegrationApiTestBase
    {
        [Fact]
        public async Task CreateUserTest()
        {
            var user = new
            {
                firstName = "Jane",
                lastName = "Doe",
                email = "jane.doe@example.com",
            };

            var content = CreateContent(user);

            var response = await SendPostRequest("/api/users/create", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendsDuplicateEmailError()
        {
            var context = Server.CreateDbContext();
            await CreateUser("John", "Doe", "john.doe@example.com", context);
            var user = new
            {
                firstName = "John",
                lastName = "Doe",
                email = "john.doe@example.com",
            };

            var content = CreateContent(user);

            var response = await SendPostRequest("/api/users/create", content);
            var result = await GetJsonObject<ValidationResponse>(response);

            var expected = CreateExpectedResponse(
                "Invalid email",
                "A user with email john.doe@example.com already exists"
            );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SendsInvalidModelError()
        {
            var user = new
            {
                firstName = "Jane",
                lastName = "Doe",
            };

            var content = CreateContent(user);
            var response = await SendPostRequest("/api/users/create", content);

            var result = await GetJsonObject<ValidationResponse>(response);

            var expected = CreateExpectedResponse(
                "Validation failed",
                "The Email field is required."
            );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }
    }
}