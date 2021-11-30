using TaskManager.Models.Query;

namespace TaskManager.Tests.Integration.ScheduledTasks
{
    public sealed class ScheduledTaskResponse
    {
        public string Id { get; set; }
        public Task Task { get; set; }
        public User User { get; set; }
        public string Preceding { get; set; }
        public string PrecedingId { get; set; }
    }
}