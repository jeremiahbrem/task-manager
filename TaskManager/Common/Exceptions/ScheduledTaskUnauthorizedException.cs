using System;
using System.Collections.Generic;
using TaskManager.Common.Validation;

namespace TaskManager.Common.Exceptions
{

    public sealed class ScheduledTaskUnauthorizedException : Exception
    {
        public ScheduledTaskUnauthorizedException(string id)
            : base("Unauthorized")
        {
            Errors = new ValidationError[] { new ($"You are not authorized to access scheduled task {id}")};
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}