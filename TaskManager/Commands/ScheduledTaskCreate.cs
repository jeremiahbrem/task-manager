using System.ComponentModel.DataAnnotations;
using TaskManager.Models.Domain.ScheduledTask;

namespace TaskManager.Commands
{
    public class ScheduledTaskCreate
    {
        [Required]
        public string Task { get; set; } = "";
        public string? PrecedingId { get; set; }

        public ScheduledTask ToCreatedScheduledTask(
            string id,
            Models.Domain.Task.Task task,
            Models.Domain.User.User user,
            int? precedingId)
        {
            return new ScheduledTask()
            {
                Task = task,
                User = user,
                PrecedingTaskId = precedingId,
                ScheduledTaskId = id,
                Completed = false
            };
        }
    }
}