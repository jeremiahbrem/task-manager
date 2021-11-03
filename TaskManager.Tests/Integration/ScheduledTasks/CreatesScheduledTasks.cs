using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Task = TaskManager.Models.Task;
using TaskManager.Models.User;
using TaskManager.Tests.Mamas;
using Xunit;

namespace TaskManager.Tests.Integration.ScheduledTasks
{
    public class CreatesScheduledTasks : IntegrationApiTestBase
    {
        private readonly User _user = new UserMother("Jane", "Doe", "jane.doe@example.com").User;
        private readonly Task.Task _task = new TaskMother("Test Task", "Category One").Task;
        private readonly Task.Task _taskTwo = new TaskMother("Test Task Two", "Category One").Task;

        [Fact]
        public async System.Threading.Tasks.Task CreateScheduledTaskTest()
        {
            var context = Server.CreateDbContext();

            await CreateTask(_task.Name, _task.Category.Name, context);
            await CreateUser(_user.FirstName, _user.LastName, _user.Email, context);
            await CreateScheduledTask(_task, _user, context);

            var precedingTask = await context.ScheduledTasks.FirstOrDefaultAsync();

            await CreateTask("Test Task Two", _task.Category.Name, context);

            var scheduledTask = new
            {
                Task = _taskTwo.Name,
                PrecedingId = precedingTask.ScheduledTaskId,
                Email = _user.Email
            };

            var content = CreateContent(scheduledTask);

            var response = await SendPostRequest("/api/scheduled-tasks/create", content);

            var newScheduledTask = await context.ScheduledTasks.FirstOrDefaultAsync(x => x.Task.Name == _taskTwo.Name);
            var task = await context.Tasks.FirstOrDefaultAsync(x => x.Name == _taskTwo.Name);
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == _user.Email);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            newScheduledTask.Should().BeEquivalentTo(new
            {
                Task = task,
                User = user,
                PrecedingTask = precedingTask
            });
        }
    }
}