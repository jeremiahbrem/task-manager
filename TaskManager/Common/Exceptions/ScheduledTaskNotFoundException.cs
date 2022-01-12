using System;
using System.Collections.Generic;
using TaskManager.Common.Validation;

namespace TaskManager.Common.Exceptions
{

    public sealed class ScheduledTaskNotFoundException : Exception
    {
        public ScheduledTaskNotFoundException(string id)
            : base("Invalid scheduled task id")
        {
            Errors = new ValidationError[] { new ($"A scheduled task with id {id} was not found")};
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}