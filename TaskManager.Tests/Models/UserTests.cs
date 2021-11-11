using FluentAssertions;
using TaskManager.Models.Domain.User;
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

        [Fact]
        public void ToQueryObjectTest()
        {
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var result = user.ToQueryObject();

            result.Should().BeEquivalentTo(new
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            });
        }
    }
}