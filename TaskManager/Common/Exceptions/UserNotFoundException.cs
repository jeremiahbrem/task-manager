using System;
using System.Collections.Generic;
using TaskManager.Common.Validation;

namespace TaskManager.Common.Exceptions
{

    public sealed class UserNotFoundException : Exception
    {
        public UserNotFoundException(string email)
            : base("Invalid email")
        {
            Errors = new ValidationError[] { new ($"A user with email {email} was not found")};
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}