using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Common.Validation;
using TaskManager.Common.Validation.ValidationModel;
using TaskManager.Database;
using TaskManager.Models.Categories;
using Task = TaskManager.Models.Task;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class TasksController : Controller
    {
        private readonly Task.TaskRepository _repo;
        private readonly CategoryRepository _categoryRepo;

        public TasksController(TaskManagerContext context)
        {
            _repo = new Task.TaskRepository(context);
            _categoryRepo = new CategoryRepository(context);
        }

        [HttpPost("create")]
        public async Task<ActionResult> PostCreate([FromBody]Task.TaskCreate task)
        {
            var existingCategory = await _categoryRepo.GetCategory(task.Category);

            if (existingCategory == null)
            {
                return new ValidationResult(
                    "Invalid category",
                    new List<ValidationError> { new ($"Invalid category {task.Category}. You must use an existing category.") }
                );
            }

            var existingTask = await _repo.GetTask(task.Name);

            if (existingTask != null)
            {
                return new ValidationResult(
                    "Duplicate task error",
                    new List<ValidationError> { new ($"A task with name {task.Name} already exists.") }
                );
            }

            var createdTask = task.ToCreatedTask(existingCategory);

            await _repo.AddTask(createdTask);

            return new ValidationResult($"Task {task} created.");
        }
    }
}