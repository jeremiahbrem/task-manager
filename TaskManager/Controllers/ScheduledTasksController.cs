using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Common.Validation;
using TaskManager.Common.Validation.ValidationModel;
using TaskManager.Database;
using TaskManager.Models.ScheduledTask;
using TaskManager.Models.User;
using Task = TaskManager.Models.Task;

namespace TaskManager.Controllers
{
    [Route("api/scheduled-tasks")]
    [ValidateModel]
    public class ScheduledTasksController : Controller
    {
        private readonly ScheduledTaskRepository _repo;
        private readonly Task.TaskRepository _taskRepo;
        private readonly UserRepository _userRepo;

        public ScheduledTasksController(TaskManagerContext context)
        {
            _repo = new ScheduledTaskRepository(context);
            _taskRepo = new Task.TaskRepository(context);
            _userRepo = new UserRepository(context);
        }

        [HttpPost("create")]
        public async Task<ActionResult> PostCreate([FromBody]ScheduledTaskCreate scheduledTask)
        {
            var existingTask = await _taskRepo.GetTask(scheduledTask.Task);

            if (existingTask == null)
            {
                return new ValidationResult(
                    "Invalid task",
                    new List<ValidationError> { new ($"Invalid task {scheduledTask.Task}. You must use an existing task.") }
                );
            }

            var existingUser = await _userRepo.GetUser(scheduledTask.Email);

            if (existingUser == null)
            {
                return new ValidationResult(
                    "Invalid user",
                    new List<ValidationError> { new ($"A user with email {scheduledTask.Email} does not exist.") }
                );
            }

            var precedingId = 0;

            if (scheduledTask.PrecedingId != null)
            {
                var existingScheduledTask = await _repo.GetScheduledTask(scheduledTask.PrecedingId);

                if (existingScheduledTask == null)
                {
                    return new ValidationResult(
                        "Invalid preceding id",
                        new List<ValidationError> { new ($"A scheduled task with id {scheduledTask.PrecedingId} does not exist.") }
                    );
                }

                precedingId = existingScheduledTask.Id;
            }

            var createdScheduledTask = scheduledTask.ToCreatedScheduledTask(
                Guid.NewGuid().ToString(),
                existingTask,
                existingUser,
                precedingId > 0 ? precedingId : null
            );

            await _repo.AddScheduledTask(createdScheduledTask);

            return new ValidationResult($"ScheduledTask {createdScheduledTask.ScheduledTaskId} created.");
        }
    }
}