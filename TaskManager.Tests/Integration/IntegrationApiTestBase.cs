using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TaskManager.Common.Validation;
using TaskManager.Database;
using TaskManager.Models.Domain.ScheduledTask;
using TaskManager.Models.Domain.User;
using TaskManager.Tests.Mamas;
using Task = TaskManager.Models.Domain.Task.Task;

namespace TaskManager.Tests.Integration
{
    public class IntegrationApiTestBase : IntegrationTestBase
    {
        protected static ByteArrayContent CreateContent(object content)
        {
            var json = JsonConvert.SerializeObject(content);
            var buffer = Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
        protected static ValidationResponse CreateExpectedResponse(string message, string error = null)
        {
            return new ValidationResponse
            {
                Message = message,
                Errors = error != null ? new List<ValidationError> { new(error) } : null
            };
        }

        protected async Task<HttpResponseMessage> SendPostRequest(string path, ByteArrayContent content, string email = null)
        {
            var client = Server.CreateClient();
            if (email != null)
            {
                client.DefaultRequestHeaders.Add("email", email);
            }
            var response = await client.PostAsync(path, content);
            return response;
        }

        protected async Task<HttpResponseMessage> SendGetRequest(string path, string email = null)
        {
            var client = Server.CreateClient();
            if (email != null)
            {
                client.DefaultRequestHeaders.Add("email", email);
            }
            var response = await client.GetAsync(path);
            return response;
        }

        protected static async Task<T> GetJsonObject<T>(HttpResponseMessage response) where T : class
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }

        protected static async Task<JArray> GetJArray(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<JArray>(json);
            return result;
        }

        protected static async Task<IEnumerable<T>> GetJsonObjectArray<T>(HttpResponseMessage response) where T : class
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<JArray>(json);
            return result.Select(x => JsonConvert.DeserializeObject<T>(x.ToString()));
        }

        protected static async Task<User> CreateUser(string firstName, string lastName, string email, TaskManagerContext context)
        {
            var user = new UserMother(firstName, lastName, email).User;
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        protected static async Task<Task> CreateTask(string taskName, string categoryName, TaskManagerContext context)
        {
            var task = new TaskMother(taskName, categoryName).Task;
            context.Categories.Add(task.Category);
            context.Tasks.Add(task);
            await context.SaveChangesAsync();
            return task;
        }

        protected static async Task<ScheduledTask> CreateScheduledTask(
            Task task,
            User user,
            TaskManagerContext context,
            ScheduledTask preceding = null,
            string id = null)
        {
            var createdTask = await CreateTask(task.Name, task.Category.Name, context);
            var scheduledTask = new ScheduledTaskMother(createdTask, user, preceding, id).ScheduledTask;
            context.ScheduledTasks.Add(scheduledTask);
            await context.SaveChangesAsync();
            return scheduledTask;
        }

        protected static async System.Threading.Tasks.Task CreateCategory(string name, TaskManagerContext context)
        {
            var mother = new CategoryMother(name);
            var category = mother.Category;
            context.Categories.Add(category);
            await context.SaveChangesAsync();
        }
    }
}