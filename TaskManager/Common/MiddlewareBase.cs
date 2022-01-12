using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TaskManager.Common.Validation;

namespace TaskManager.Common
{
    public abstract class MiddlewareBase
    {
        public ValidationResponse CreateValidationResponse(string message, string error)
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

        public async Task SendResponse(HttpContext context, string message, IEnumerable<ValidationError> errors, int statusCode)
        {
            var response = CreateValidationResponse(message, errors);
            await CreateResponseMessage(context, statusCode, response);
        }

        public ValidationResponse CreateValidationResponse(string message, IEnumerable<ValidationError> errors)
        {
            return new ValidationResponse
            {
                Message = message,
                Errors = errors
            };
        }

        public async Task CreateResponseMessage(HttpContext context, int statusCode, ValidationResponse response)
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