using System;

namespace TaskManager.Models.Domain.ScheduledTask
{
    public class ScheduledTask
    {
        public int Id { get; set; }
        public string ScheduledTaskId { get; set; } = "";
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public int? PrecedingTaskId { get; set; }
        public Task.Task Task { get; set; } = null!;
        public ScheduledTask? PrecedingTask { get; set; }
        public User.User User { get; set; } = null!;

        public Query.ScheduledTask ToQueryObject()
        {
            return new Query.ScheduledTask
            {
                Id = ScheduledTaskId,
                Task = Task.ToQueryObject(),
                User = User.ToQueryObject(),
                Preceding = PrecedingTask?.Task.Name,
                PrecedingId = PrecedingTask?.ScheduledTaskId
            };
        }
    }
}