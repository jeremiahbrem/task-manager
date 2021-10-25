using System;

namespace TaskManager.Models.ScheduledTask
{
    public class ScheduledTaskCreate
    {
        public string Email { get; set; } = "";
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