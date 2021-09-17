using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Validation;
using TaskManager.Common.Validation.ValidationModel;
using TaskManager.Database;
using TaskManager.Models;
using TaskManager.Models.Categories;

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
            var existingCategory = await _repo.GetCategory(category.Name);

            if (existingCategory != null)
            {
                return new ValidationResult(
                    "Duplicate category error",
                    new List<ValidationError> { new ($"A category with name {category.Name} already exists.") }
                );
            }

            await _repo.AddCategory(category);

            return new ValidationResult($"Category {category.Name} created.");
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