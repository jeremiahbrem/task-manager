using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManager.Common.Validation;
using TaskManager.Common.Validation.ValidationModel;
using TaskManager.Controllers;
using Xunit;

namespace TaskManager.Tests.Validation
{
    public class ValidationModelTests : IntegrationTestBase
    {
        private readonly TasksController _controller;

        public ValidationModelTests()
        {
            var context = Server.CreateDbContext();
            _controller = new TasksController(context);
        }

        [Fact]
        public void TestValidationModelResponse()
        {
            _controller.ModelState.AddModelError("Error", "There was an error.");
            var response = new ValidationModelResponse(_controller.ModelState);

            Assert.Equal("Validation failed", response.Message);
            response.Errors.Should().ContainEquivalentOf(new ValidationError("There was an error."));
            Assert.Single(response.Errors);
        }

        [Fact]
        public void TestValidationModelResult()
        {
            _controller.ModelState.AddModelError("Error", "There was an error.");
            var response = new ValidationModelResult(_controller.ModelState);
            var result = response.Value;

            Assert.Equal(400, response.StatusCode);
            result.Should().BeEquivalentTo(new ValidationModelResponse(_controller.ModelState));
        }
    }
}
