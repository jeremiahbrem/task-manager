using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TaskManager.Common.Validation.ValidationModel
{
    public class ValidationModelResult : ObjectResult
    {
        public ValidationModelResult(ModelStateDictionary modelState)
            : base(new ValidationModelResponse(modelState))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}