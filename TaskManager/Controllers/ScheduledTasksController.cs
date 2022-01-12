using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Auth;
using TaskManager.Common.Validation;
using TaskManager.Common.Validation.ValidationModel;
using TaskManager.Database;
using TaskManager.Models.Domain.ScheduledTask;
using TaskManager.Repositories;

namespace TaskManager.Controllers
{
    [Route("api/scheduled-tasks")]
    [ValidateModel]
    public class ScheduledTasksController : Controller
    {
        private readonly ScheduledTaskRepository _repo;
        private readonly TaskRepository _taskRepo;
        private readonly UserRepository _userRepo;
        private readonly CurrentUser _user;
        private readonly TaskManagerContext _context;

        public ScheduledTasksController(TaskManagerContext context, CurrentUser user)
        {
            _repo = new ScheduledTaskRepository(context, user);
            _taskRepo = new TaskRepository(context);
            _userRepo = new UserRepository(context);
            _user = user;
            _context = context;
        }

        [HttpPost("create")]
        public async Task<ActionResult> PostCreate([FromBody]ScheduledTaskCreate scheduledTask)
        {
            var existingTask = await _taskRepo.GetTask(scheduledTask.Task);
            var existingUser = await _userRepo.GetUser(_user.Email!);

            var precedingId = 0;

            if (scheduledTask.PrecedingId != null)
            {
                var existingScheduledTask = await _repo.GetScheduledTask(scheduledTask.PrecedingId);
                await _repo.VerifyUserEmail(scheduledTask.PrecedingId);
                precedingId = existingScheduledTask!.Id;
            }

            var createdScheduledTask = scheduledTask.ToCreatedScheduledTask(
                Guid.NewGuid().ToString(),
                existingTask!,
                existingUser!,
                precedingId > 0 ? precedingId : null
            );

            await _repo.AddScheduledTask(createdScheduledTask);
            return new ValidationResult($"Scheduled task {createdScheduledTask.ScheduledTaskId} created.", 200);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTask(string id)
        {
            var existingScheduledTask = await _repo.GetScheduledTask(id);
            await _repo.VerifyUserEmail(id);

            return new JsonResult(existingScheduledTask!.ToQueryObject());
        }

        [HttpGet]
        public async Task<ActionResult> GetScheduledTasks()
        {
            var filteredCategory = HttpContext.Request.Query["category"];
            List<ScheduledTask> result;

            if (!string.IsNullOrEmpty(filteredCategory))
            {
                result = await _repo.GetScheduledTasks(filteredCategory);
            }
            else
            {
                result = await _repo.GetScheduledTasks();
            }
            var tasks = result.Select(x => x.ToQueryObject()).ToArray();

            return new JsonResult(tasks);
        }

        [HttpPost("complete/{id}")]
        public async Task<ActionResult> CompleteScheduledTask(string id)
        {
            var existingScheduledTask = await _repo.GetScheduledTask(id);
            await _repo.VerifyUserEmail(id);

            existingScheduledTask!.Complete();
            await _repo.UpdateScheduledTask(existingScheduledTask);

            return new ValidationResult($"Scheduled task {id} completed.", 200);
        }
    }
}