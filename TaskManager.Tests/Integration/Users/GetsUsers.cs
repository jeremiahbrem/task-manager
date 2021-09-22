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
            var response = await SendGetRequest("/api/users");
            var result = await GetJArray(response);

            var expected = new List<JObject>
            {
                new (
                    new JProperty("firstName", "John"),
                    new JProperty("lastName", "Doe"),
                    new JProperty("email", "john.doe@example.com")
                )
            };

            result.Should().BeEquivalentTo(expected);
        }
    }
}