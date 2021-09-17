using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TaskManager.Common.Validation.ValidationModel
{
    public class ValidationModelResponse
    {
        public string Message { get; }

        public IEnumerable<ValidationError> Errors { get; }

        public ValidationModelResponse(ModelStateDictionary modelState)
        {
            Message = "Validation failed";
            Errors = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(x.ErrorMessage)))
                .ToList();
        }
    }
}