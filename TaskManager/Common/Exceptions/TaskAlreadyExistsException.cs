using System;
using System.Collections.Generic;
using TaskManager.Common.Validation;

namespace TaskManager.Common.Exceptions
{

    public sealed class TaskAlreadyExistsException : Exception
    {
        public TaskAlreadyExistsException(string name)
            : base("Invalid task name")
        {
            Errors = new ValidationError[] { new ($"A task with name {name} already exists")};
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}