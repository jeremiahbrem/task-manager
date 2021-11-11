using FluentAssertions;
using TaskManager.Models.Domain.Categories;
using Xunit;

namespace TaskManager.Tests.Models
{
    public class CategoryTests
    {
        [Fact]
        public void ToQueryObjectTest()
        {
            var category = new Category
            {
                Name = "Maintentance",
            };

            var result = category.ToQueryObject();

            result.Should().BeEquivalentTo(new
            {
                Name = "Maintentance"
            });
        }
    }
}