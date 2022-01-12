using System;
using System.Collections.Generic;

namespace TaskManager.Common.Validation
{

    public sealed class ValidationException : Exception
    {
        public ValidationException(string message, params ValidationError[] errors)
            : base(message)
        {
            Errors = errors;
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}