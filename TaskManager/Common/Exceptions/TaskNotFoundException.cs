using System;
using System.Collections.Generic;
using TaskManager.Common.Validation;

namespace TaskManager.Common.Exceptions
{

    public sealed class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(string name)
            : base("Invalid task name")
        {
            Errors = new ValidationError[] { new ($"A task with name {name} was not found")};
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}