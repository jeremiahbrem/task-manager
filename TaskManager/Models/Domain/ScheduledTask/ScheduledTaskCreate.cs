using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.Domain.ScheduledTask
{
    public class ScheduledTaskCreate
    {
        [Required]
        public string Email { get; set; } = "";
        [Required]
        public string Task { get; set; } = "";
        public string? PrecedingId { get; set; }

        public ScheduledTask ToCreatedScheduledTask(
            string id,
            Task.Task task,
            User.User user,
            int? precedingId)
        {
            return new ScheduledTask()
            {
                Task = task,
                User = user,
                PrecedingTaskId = precedingId,
                ScheduledTaskId = id,
            };
        }
    }
}