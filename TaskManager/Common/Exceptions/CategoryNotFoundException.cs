using System;
using System.Collections.Generic;
using TaskManager.Common.Validation;

namespace TaskManager.Common.Exceptions
{

    public sealed class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException(string name)
            : base("Invalid category name")
        {
            Errors = new ValidationError[] { new ($"A category with name {name} was not found")};
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}