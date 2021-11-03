using System;
using TaskManager.Models.ScheduledTask;
using TaskManager.Models.Task;
using TaskManager.Models.User;

namespace TaskManager.Tests.Mamas
{
    public class ScheduledTaskMother : IntegrationTestBase
    {
        private readonly Task _task;
        private readonly User _user;
        private readonly ScheduledTask _preceding;
        public ScheduledTask ScheduledTask => CreateScheduledTask();

        public ScheduledTaskMother(Task task, User user, ScheduledTask preceding = null)
        {
            _task = task;
            _user = user;
            _preceding = preceding;
        }

        private ScheduledTask CreateScheduledTask()
        {
            return new ScheduledTask()
            {
                Task = _task,
                User = _user,
                PrecedingTask = _preceding,
                ScheduledTaskId = Guid.NewGuid().ToString()
            };
        }
    }
}