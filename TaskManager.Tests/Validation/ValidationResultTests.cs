using System.Collections.Generic;
using FluentAssertions;
using TaskManager.Common.Validation;
using Xunit;

namespace TaskManager.Tests.Validation
{
    public class ValidationResultTests
    {
        [Fact]
        public void TestValidationError()
        {
            var error = new ValidationError("There was an error");
            Assert.Equal("There was an error", error.Message);
        }

        [Fact]
        public void TestValidationResultError()
        {
            var result = new ValidationResult("Error", new List<ValidationError>{ new ("There was an error") });

            result.Value.Should().BeEquivalentTo(new ValidationResponse
            {
                Message = "Error",
                Errors = new List<ValidationError> {new("There was an error")}
            });

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void TestValidationResultSuccess()
        {
            var result = new ValidationResult("Task created");

            result.Value.Should().BeEquivalentTo(new ValidationResponse
            {
                Message = "Task created",
                Errors = null
            });

            Assert.Equal(200, result.StatusCode);
        }
    }
}