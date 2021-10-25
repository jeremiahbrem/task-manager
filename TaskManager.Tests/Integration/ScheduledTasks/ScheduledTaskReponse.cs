using TaskManager.Models.ScheduledTask;
using TaskManager.Models.Task;
using TaskManager.Models.User;

namespace TaskManager.Tests.Integration.ScheduledTasks
{
    public sealed class ScheduledTaskResponse
    {
        public Task Task { get; set; }
        public User User { get; set; }
        public ScheduledTask PrecedingTask { get; set; }
    }
}