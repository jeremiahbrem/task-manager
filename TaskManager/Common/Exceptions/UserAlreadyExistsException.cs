using System;
using System.Collections.Generic;
using TaskManager.Common.Validation;

namespace TaskManager.Common.Exceptions
{

    public sealed class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string email)
            : base("Invalid email")
        {
            Errors = new ValidationError[] { new ($"A user with email {email} already exists")};
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}