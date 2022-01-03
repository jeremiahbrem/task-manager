using System.Collections.Generic;
using FluentAssertions;
using TaskManager.Common.Validation;
using Xunit;

namespace TaskManager.Tests.Validation
{
    public class ValidationResultTests
    {
        [Fact]
        public void TestValidationResultError()
        {
            var result = new ValidationResult("Error", 400, new List<ValidationError>{ new ("There was an error") });

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
            var result = new ValidationResult("Task created", 200);

            result.Value.Should().BeEquivalentTo(new ValidationResponse
            {
                Message = "Task created",
                Errors = null
            });

            Assert.Equal(200, result.StatusCode);
        }
    }
}