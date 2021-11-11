namespace TaskManager.Models.Query
{
    public class ScheduledTask
    {
        public Task Task { get; set; } = null!;
        public User User { get; set; } = null!;
        public ScheduledTask? Preceding { get; set; }
    }
}