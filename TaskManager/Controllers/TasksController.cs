using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Commands;
using TaskManager.Common.Validation;
using TaskManager.Common.Validation.ValidationModel;
using TaskManager.Database;
using TaskManager.Models.Domain.Task;
using TaskManager.Repositories;
using Task = TaskManager.Models.Domain.Task.Task;

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

            await _repo.CheckIfExists(task.Name);

            var createdTask = task.ToCreatedTask(existingCategory!);

            await _repo.AddTask(createdTask);

            return new ValidationResult($"Task {task.Name} created.", 200);
        }

        [HttpGet]
        public async Task<ActionResult> GetTasks()
        {
            var filteredCategory = HttpContext.Request.Query["category"];
            List<Task> result;

            if (!string.IsNullOrEmpty(filteredCategory))
            {
                result = await _repo.GetTasks(filteredCategory);
            }
            else
            {
                result = await _repo.GetTasks();
            }

            var tasks = result.Select(x => x.ToQueryObject()).ToArray();

            return new JsonResult(tasks);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult> GetTask(string name)
        {
            var task = await _repo.GetTask(name);

            return new JsonResult(task!.ToQueryObject());
        }
    }
}