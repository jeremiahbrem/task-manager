using System;
using System.Collections.Generic;
using TaskManager.Common.Validation;

namespace TaskManager.Common.Exceptions
{

    public sealed class CategoryAlreadyExistsException : Exception
    {
        public CategoryAlreadyExistsException(string name)
            : base("Invalid category name")
        {
            Errors = new ValidationError[] { new ($"A category with name {name} already exists")};
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}