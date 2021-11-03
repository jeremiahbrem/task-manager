using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TaskManager.Tests.Integration.Users
{
    public class GetsUser : IntegrationApiTestBase
    {
        [Fact]
        public async Task GetsUserTest()
        {
            var context = Server.CreateDbContext();
            await CreateUser("John", "Doe", "john.doe@example.com", context);
            var response = await SendGetRequest("/api/users/john.doe@example.com");
            var result = await GetJsonObject<UserResponse>(response);

            var expected = new UserResponse
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ReturnsNotFound()
        {
            var response = await SendGetRequest("/api/users/unknown@example.com");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}