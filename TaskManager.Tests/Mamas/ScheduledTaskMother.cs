using System;
using TaskManager.Models.Domain.ScheduledTask;
using TaskManager.Models.Domain.Task;
using TaskManager.Models.Domain.User;

namespace TaskManager.Tests.Mamas
{
    public class ScheduledTaskMother : IntegrationTestBase
    {
        private readonly Task _task;
        private readonly User _user;
        private readonly ScheduledTask _preceding;
        private readonly string _id;
        public ScheduledTask ScheduledTask => CreateScheduledTask();

        public ScheduledTaskMother(Task task, User user, ScheduledTask preceding = null, string id = null)
        {
            _task = task;
            _user = user;
            _preceding = preceding;
            _id = id ?? Guid.NewGuid().ToString();
        }

        private ScheduledTask CreateScheduledTask()
        {
            return new ScheduledTask()
            {
                Task = _task,
                User = _user,
                PrecedingTask = _preceding,
                ScheduledTaskId = _id
            };
        }
    }
}