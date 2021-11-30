using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace TaskManager.Tests.Integration.Users
{
    public class GetsUsers : IntegrationApiTestBase
    {
        [Fact]
        public async Task GetsAllUsers()
        {
            var context = Server.CreateDbContext();
            await CreateUser("John", "Doe", "john.doe@example.com", context);
            await CreateUser("Jane", "Doe", "jane.doe@example.com", context);
            var response = await SendGetRequest("/api/users");
            var result = await GetJsonObjectArray<UserResponse>(response);

            var expected = new List<UserResponse>
            {
                new UserResponse
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com"
                },
                new UserResponse
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = "jane.doe@example.com"
                },
            };

            result.Should().BeEquivalentTo(expected);
        }
    }
}