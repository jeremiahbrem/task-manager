using System.Threading.Tasks;
using TaskManager.Tests.Mamas;

namespace TaskManager.Tests.Integration.Tasks
{
    public abstract class TaskTestBase : IntegrationApiTestBase
    {
        protected async Task CreateTask(string taskName, string categoryName)
        {
            var task = new TaskMother(taskName, categoryName).Task;
            var context = Server.CreateDbContext();
            context.Categories.Add(task.Category);
            context.Tasks.Add(task);
            await context.SaveChangesAsync();
        }
    }
}