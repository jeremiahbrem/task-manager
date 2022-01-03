using System;
using System.Collections.Generic;

namespace TaskManager.Common.Validation
{

    public sealed class ValidationException : Exception
    {
        public ValidationException(params ValidationError[] errors)
            : base("Validation failed")
        {
            Errors = errors;
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}