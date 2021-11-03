using System.Threading.Tasks;
using TaskManager.Tests.Mamas;

namespace TaskManager.Tests.Integration.Categories
{
    public abstract class CategoryTestBase : IntegrationApiTestBase
    {
        protected async Task CreateCategory(string name)
        {
            var context = Server.CreateDbContext();
            var mother = new CategoryMother("CategoryOne");
            var category = mother.Category;
            context.Categories.Add(category);
            await context.SaveChangesAsync();
        }
    }
}