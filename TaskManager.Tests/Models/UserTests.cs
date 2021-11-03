using FluentAssertions;
using TaskManager.Models.User;
using Xunit;

namespace TaskManager.Tests.Models
{
    public class UserTests
    {
        [Fact]
        public void ToCreateUserTest()
        {
            var request = new UserCreate
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var result = request.ToCreatedUser();

            result.Should().BeEquivalentTo(new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            });
        }
    }
}