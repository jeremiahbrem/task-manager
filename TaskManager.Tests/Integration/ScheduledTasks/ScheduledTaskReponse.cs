#nullable enable
using TaskManager.Models.Query;

namespace TaskManager.Tests.Integration.ScheduledTasks
{
    public sealed class ScheduledTaskResponse
    {
        public string Id { get; set; } = null!;
        public Task Task { get; set; } = null!;
        public User User { get; set; } = null!;
        public bool Completed { get; set; }
        public string? Preceding { get; set; }
        public string? PrecedingId { get; set; }
    }
}