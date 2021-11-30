using System;
using FluentAssertions;
using TaskManager.Models.Domain.Categories;
using TaskManager.Models.Domain.ScheduledTask;
using TaskManager.Models.Domain.Task;
using TaskManager.Models.Domain.User;
using Xunit;

namespace TaskManager.Tests.Models
{
    public class ScheduledTasksTests
    {
        private readonly string _scheduledTaskId = Guid.NewGuid().ToString();
        private readonly string _precedingId = Guid.NewGuid().ToString();

        private readonly User _user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        private readonly Category _category = new Category {Name = "Maintenance"};

        private readonly Task _task = new Task
        {
            Name = "Change oil",
            Category = new Category {Name = "Maintenance"},
        };

        [Fact]
        public void ToCreateScheduledTaskTest()
        {
            var request = new ScheduledTaskCreate
            {
                Task = "Change oil",
                Email = "john.doe@example.com",
                PrecedingId = _precedingId,
            };

            var preceding = CreatePreceding();

            var result = request.ToCreatedScheduledTask(_scheduledTaskId, _task, _user, preceding.Id);

            result.Should().BeEquivalentTo(new ScheduledTask
            {
                User = _user,
                Task = _task,
                PrecedingTaskId = preceding.Id,
                ScheduledTaskId = _scheduledTaskId,
            });
        }

        [Fact]

        public void ToQueryObjectTest()
        {
            var preceding = CreatePreceding();
            var id = Guid.NewGuid().ToString();

            var scheduledTask = new ScheduledTask
            {
                Task = _task,
                User = _user,
                PrecedingTask = preceding,
                ScheduledTaskId = id
            };

            var result = scheduledTask.ToQueryObject();

            result.Should().BeEquivalentTo(new
            {
                Id =  scheduledTask.ScheduledTaskId,
                User = _user.ToQueryObject(),
                Task = _task.ToQueryObject(),
                Preceding = preceding.Task.Name,
                PrecedingId = preceding.ScheduledTaskId
            });
        }

        private ScheduledTask CreatePreceding()
        {
            return new ScheduledTask
            {
                User = _user,
                Task = new Task {Name = "Rotate tires", Category = _category},
                ScheduledTaskId = _precedingId,
                Id = 1234
            };
        }
    }
}