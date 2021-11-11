using TaskManager.Models.Domain.ScheduledTask;
using TaskManager.Models.Domain.Task;
using TaskManager.Models.Domain.User;

namespace TaskManager.Tests.Integration.ScheduledTasks
{
    public sealed class ScheduledTaskResponse
    {
        public Task Task { get; set; }
        public User User { get; set; }
        public ScheduledTask PrecedingTask { get; set; }
    }
}