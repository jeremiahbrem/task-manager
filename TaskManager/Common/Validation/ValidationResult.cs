using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Common.Validation
{
    public class ValidationResult : ObjectResult
    {
        public ValidationResult(
            string message,
            int statusCode,
            IEnumerable<ValidationError>? errors = null
        )
            : base(new ValidationResponse
            {
                Message = message,
                Errors = errors,
            })
        {
            StatusCode = statusCode;
        }
    }
}