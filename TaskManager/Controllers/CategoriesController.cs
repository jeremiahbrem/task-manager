using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Common.Validation;
using TaskManager.Common.Validation.ValidationModel;
using TaskManager.Database;
using TaskManager.Models.Domain.Categories;
using TaskManager.Repositories;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class CategoriesController : Controller
    {
        private readonly CategoryRepository _repo;

        public CategoriesController(TaskManagerContext context)
        {
            _repo = new CategoryRepository(context);
        }

        [HttpPost("create")]
        public async Task<ActionResult> PostCreate([FromBody]Category category)
        {
            await _repo.CheckIfExists(category.Name);

            await _repo.AddCategory(category);

            return new ValidationResult($"Category {category.Name} created.", 200);
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _repo.GetCategories();

            var categories = result.Select(x => x.Name).ToArray();

            return new JsonResult(categories);
        }
    }
}