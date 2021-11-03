using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TaskManager.Common.Validation;
using TaskManager.Database;
using TaskManager.Models.ScheduledTask;
using TaskManager.Models.User;
using TaskManager.Tests.Mamas;
using TaskModel = TaskManager.Models.Task;

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
        protected static ValidationResponse CreateExpectedResponse(string message, string error)
        {
            return new ValidationResponse
            {
                Message = message,
                Errors = new List<ValidationError> { new(error) }
            };
        }

        protected async Task<HttpResponseMessage> SendPostRequest(string path, ByteArrayContent content)
        {
            var client = Server.CreateClient();
            var response = await client.PostAsync(path, content);
            return response;
        }

        protected async Task<HttpResponseMessage> SendGetRequest(string path)
        {
            var client = Server.CreateClient();
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

        protected static async Task CreateUser(string firstName, string lastName, string email, TaskManagerContext context)
        {
            var user = new UserMother(firstName, lastName, email).User;
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        protected static async Task CreateTask(string taskName, string categoryName, TaskManagerContext context)
        {
            var task = new TaskMother(taskName, categoryName).Task;
            context.Categories.Add(task.Category);
            context.Tasks.Add(task);
            await context.SaveChangesAsync();
        }

        protected static async Task CreateScheduledTask(
            TaskModel.Task task,
            User user,
            TaskManagerContext context,
            ScheduledTask preceding = null)
        {
            var scheduledTask = new ScheduledTaskMother(task, user, preceding).ScheduledTask;
            context.ScheduledTasks.Add(scheduledTask);
            await context.SaveChangesAsync();
        }

        protected static async Task CreateCategory(string name, TaskManagerContext context)
        {
            var mother = new CategoryMother(name);
            var category = mother.Category;
            context.Categories.Add(category);
            await context.SaveChangesAsync();
        }
    }
}