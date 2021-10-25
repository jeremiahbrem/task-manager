namespace TaskManager.Models.ScheduledTask
{
    public class ScheduledTask
    {
        public int Id { get; set; }
        public string ScheduledTaskId { get; set; } = null!;
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public int? PrecedingTaskId { get; set; }

        public Task.Task Task { get; set; } = null!;
        public ScheduledTask? PrecedingTask { get; set; }
        public User.User User { get; set; } = null!;
    }
}