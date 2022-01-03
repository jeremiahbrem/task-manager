using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Common.Validation;
using TaskManager.Common.Validation.ValidationModel;
using TaskManager.Database;
using TaskManager.Models.Domain.Categories;
using TaskManager.Models.Domain.Task;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class TasksController : Controller
    {
        private readonly TaskRepository _repo;
        private readonly CategoryRepository _categoryRepo;

        public TasksController(TaskManagerContext context)
        {
            _repo = new TaskRepository(context);
            _categoryRepo = new CategoryRepository(context);
        }

        [HttpPost("create")]
        public async Task<ActionResult> PostCreate([FromBody]TaskCreate task)
        {
            var existingCategory = await _categoryRepo.GetCategory(task.Category);

            if (existingCategory == null)
            {
                return new ValidationResult(
                    "Invalid category",
                    400,
                    new List<ValidationError> { new ($"Invalid category {task.Category}. You must use an existing category.") }
                );
            }

            var existingTask = await _repo.GetTask(task.Name);

            if (existingTask != null)
            {
                return new ValidationResult(
                    "Duplicate task error",
                    400,
                    new List<ValidationError> { new ($"A task with name {task.Name} already exists.") }
                );
            }

            var createdTask = task.ToCreatedTask(existingCategory);

            await _repo.AddTask(createdTask);

            return new ValidationResult($"Task {task.Name} created.", 200);
        }

        [HttpGet]
        public async Task<ActionResult> GetTasks()
        {
            var result = await _repo.GetTasks();

            var tasks = result.Select(x => x.ToQueryObject()).ToArray();

            return new JsonResult(tasks);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult> GetTask(string name)
        {
            var task = await _repo.GetTask(name);

            if (task == null)
            {
                return new ValidationResult(
                    "Invalid id",
                    404,
                    new List<ValidationError> { new ($"A task with name {name} does not exist.") }
                );
            }

            return new JsonResult(task.ToQueryObject());
        }
    }
}