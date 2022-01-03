using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TaskManager.Common.Validation;
using TaskManager.Database;
using TaskManager.Models.Domain.User;

namespace TaskManager.Auth
{
    public sealed class ScheduledTaskMiddleware
    {
        private readonly RequestDelegate _next;

        public ScheduledTaskMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TaskManagerContext dbContext, CurrentUser user)
        {
            var path = context.Request.Path.ToString();

            if (!path.Contains("scheduled-tasks"))
            {
                await _next(context);
                return;
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                var response = CreateValidationResponse(
                    "Missing email",
                    "You must include an email address");

                await CreateResponseMessage(context, 400, response);
                return;
            }

            var existing = await new UserRepository(dbContext).GetUser(user.Email);

            if (existing == null)
            {
                var response = CreateValidationResponse(
                    "Unauthorized",
                    $"A user with email {user.Email} does not exist");

                await CreateResponseMessage(context, 401, response);
            }
            else
            {
                await _next(context);
            }
        }

        private ValidationResponse CreateValidationResponse(string message, string error)
        {
            return new ValidationResponse
            {
                Message = message,
                Errors = new[]
                {
                    new ValidationError(error)
                }
            };
        }

        private async Task CreateResponseMessage(HttpContext context, int statusCode, ValidationResponse response)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = MediaTypeNames.Application.Json;
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, settings));
        }
    }
}