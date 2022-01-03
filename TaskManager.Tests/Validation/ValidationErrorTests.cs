using System;
using System.Linq;
using FluentAssertions;
using TaskManager.Common.Validation;
using Xunit;

namespace TaskManager.Tests.Validation
{
    public class ValidationErrorTests
    {
        [Fact]
        public void TestValidationError()
        {
            var error = new ValidationError("There was an error");
            Assert.Equal("There was an error", error.Message);
        }

        [Fact]
        public void TestToException()
        {
            var error = new ValidationError("There was an error");
            var func = new Action(() => throw error.ToException());
            func.Should().Throw<ValidationException>()
                .And.Errors.Should().BeEquivalentTo(new object[]
                {
                    new { Message = "There was an error" }
                });
        }
    }
}