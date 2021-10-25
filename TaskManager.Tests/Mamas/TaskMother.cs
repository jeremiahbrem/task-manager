using TaskManager.Models.Categories;
using TaskManager.Models.Task;

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