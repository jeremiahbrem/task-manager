using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TaskManager.Auth;
using TaskManager.Common;
using TaskManager.Common.Exceptions;
using TaskManager.Database;
using TaskManager.Repositories;

namespace TaskManager.Common.Exceptions
{
    public sealed class ExceptionMiddleware : MiddlewareBase
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TaskManagerContext dbContext, CurrentUser user)
        {
            var path = context.Request.Path.ToString();

            try
            {
                await _next(context);
            }
            catch (TaskNotFoundException e)
            {
                var statusCode = path.Contains("scheduled-tasks") ? 400 : 404;
                await SendResponse(context, e.Message, e.Errors, statusCode);
            }
            catch (CategoryNotFoundException e)
            {
                await SendResponse(context, e.Message, e.Errors, 400);
            }
            catch (UserNotFoundException e)
            {
                var statusCode = path.Contains("users") ? 404 : 400;
                await SendResponse(context, e.Message, e.Errors, statusCode);
            }
            catch (TaskAlreadyExistsException e)
            {
                await SendResponse(context, e.Message, e.Errors, 400);
            }
            catch (CategoryAlreadyExistsException e)
            {
                await SendResponse(context, e.Message, e.Errors, 400);
            }
            catch (UserAlreadyExistsException e)
            {
                await SendResponse(context, e.Message, e.Errors, 400);
            }
            catch (ScheduledTaskNotFoundException e)
            {
                var statusCode = path.Contains("scheduled-tasks/create") ? 400 : 404;
                await SendResponse(context, e.Message, e.Errors, statusCode);
            }
            catch (ScheduledTaskUnauthorizedException e)
            {
                await SendResponse(context, e.Message, e.Errors, 403);
            }
            catch (PrecedingNotCompleteException e)
            {
                await SendResponse(context, e.Message, e.Errors, 400);
            }
        }
    }
}