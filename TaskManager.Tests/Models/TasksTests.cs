using FluentAssertions;
using TaskManager.Models;
using TaskManager.Models.Categories;
using TaskManager.Models.Task;
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
    }
}