using TaskManager.Models.Domain.Categories;
using TaskManager.Models.Domain.Task;

namespace TaskManager.Tests.Mamas
{
    public class TaskMother : IntegrationTestBase
    {
        private readonly string _name;
        public Category Category;
        public Task Task => CreateTask();

        public TaskMother(string taskName, string categoryName)
        {
            _name = taskName;
            Category = new CategoryMother(categoryName).Category;
        }

        private Task CreateTask()
        {
            return new Task
            {
                Name = _name,
                Category = Category
            };
        }
    }
}