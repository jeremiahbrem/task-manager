using FluentAssertions;
using TaskManager.Models.Domain.Categories;
using TaskManager.Models.Domain.Task;
using Xunit;

namespace TaskManager.Tests.Models
{
    public class TasksTests
    {
        [Fact]
        public void ToCreateTaskTest()
        {
            var request = new TaskCreate
            {
                Name = "Change oil",
                Category = "Maintentance",
            };

            var category = new Category
            {
                Name = "Maintentance",
            };

            var result = request.ToCreatedTask(category);

            result.Should().BeEquivalentTo(new Task
            {
                Name = "Change oil",
                Category = category,
            });
        }

        [Fact]
        public void ToQueryObjectTest()
        {
            var category = new Category
            {
                Name = "Maintentance",
            };

            var task = new Task
            {
                Name = "Change oil",
                Category = category,
            };

            var result = task.ToQueryObject();

            result.Should().BeEquivalentTo(new
            {
                Name = "Change oil",
                Category = category.ToQueryObject(),
            });
        }
    }
}