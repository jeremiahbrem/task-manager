using System;
using FluentAssertions;
using TaskManager.Models.Categories;
using TaskManager.Models.ScheduledTask;
using TaskManager.Models.Task;
using TaskManager.Models.User;
using Xunit;

namespace TaskManager.Tests.Models
{
    public class ScheduledTasksTests
    {
        [Fact]
        public void ToCreateScheduledTaskTest()
        {
            var scheduledTaskId = Guid.NewGuid().ToString();
            var precedingId = Guid.NewGuid().ToString();

            var request = new ScheduledTaskCreate
            {
                Task = "Change oil",
                Email = "john.doe@example.com",
                PrecedingId = precedingId,
            };

            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var category = new Category {Name = "Maintenance"};

            var task = new Task
            {
                Name = "Change oil",
                Category = category,
            };

            var preceding = new ScheduledTask
            {
                User = user,
                Task = new Task { Name = "Rotate tires", Category = category },
                ScheduledTaskId = precedingId,
                Id = 1234
            };

            var result = request.ToCreatedScheduledTask(scheduledTaskId, task, user, preceding.Id);

            result.Should().BeEquivalentTo(new ScheduledTask
            {
                User = user,
                Task = task,
                PrecedingTaskId = preceding.Id,
                ScheduledTaskId = scheduledTaskId,
            });
        }
    }
}