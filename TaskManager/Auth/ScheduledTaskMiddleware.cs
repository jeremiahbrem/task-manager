using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TaskManager.Common;
using TaskManager.Database;

namespace TaskManager.Auth
{
    public sealed class ScheduledTaskMiddleware : MiddlewareBase
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

            await _next(context);
        }
    }
}