using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Auth;
using TaskManager.Common.Validation;
using TaskManager.Common.Validation.ValidationModel;
using TaskManager.Database;
using TaskManager.Models.Domain.ScheduledTask;
using TaskManager.Models.Domain.Task;
using TaskManager.Models.Domain.User;

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

            if (existingTask == null)
            {
                return new ValidationResult(
                    "Invalid task",
                    400,
                    new List<ValidationError> { new ($"Invalid task {scheduledTask.Task}. You must use an existing task.") }
                );
            }

            var existingUser = await _userRepo.GetUser(_user.Email!);

            var precedingId = 0;

            if (scheduledTask.PrecedingId != null)
            {
                var existingScheduledTask = await _repo.GetScheduledTask(scheduledTask.PrecedingId);

                if (existingScheduledTask == null)
                {
                    return new ValidationResult(
                        "Invalid preceding id",
                        400,
                        new List<ValidationError> { new ($"A scheduled task with id {scheduledTask.PrecedingId} does not exist.") }
                    );
                }

                precedingId = existingScheduledTask.Id;
            }

            var createdScheduledTask = scheduledTask.ToCreatedScheduledTask(
                Guid.NewGuid().ToString(),
                existingTask,
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

            if (existingScheduledTask == null)
            {
                return new ValidationResult(
                    "Invalid id",
                    404,
                    new List<ValidationError> { new ($"A scheduled task with id {id} does not exist.") }
                );
            }

            var validUser = await _repo.GetUserEmail(id);

            if (validUser != _user.Email)
            {
                return new ValidationResult(
                    "Unauthorized",
                    401,
                    new List<ValidationError> { new ("You are not authorized to access this scheduled task.") }
                );
            }

            return new JsonResult(existingScheduledTask.ToQueryObject());
        }

        [HttpGet]
        public async Task<ActionResult> GetScheduledTasks()
        {
            var result = await _repo.GetScheduledTasks();
            var tasks = result.Select(x => x.ToQueryObject()).ToArray();

            return new JsonResult(tasks);
        }

        [HttpPost("complete")]
        public async Task<ActionResult> CompleteScheduledTask([FromBody]ScheduledTaskComplete scheduledTask)
        {
            var id = scheduledTask.Id;

            var existingScheduledTask = await _repo.GetScheduledTask(id);

            if (existingScheduledTask == null)
            {
                return new ValidationResult(
                    "Invalid id",
                    404,
                    new List<ValidationError> { new ($"A scheduled task with id {id} does not exist.") }
                );
            }

            var validUser = await _repo.GetUserEmail(id);

            if (validUser != _user.Email)
            {
                return new ValidationResult(
                    "Unauthorized",
                    401,
                    new List<ValidationError> { new ("You are not authorized to access this scheduled task.") }
                );
            }

            try
            {
                existingScheduledTask.Complete();
            }
            catch (ValidationException e)
            {
                return new ValidationResult(e.Message, 400, e.Errors);
            }

            await _repo.UpdateScheduledTask(existingScheduledTask);

            return new ValidationResult($"Scheduled task {id} completed.", 200);
        }
    }
}