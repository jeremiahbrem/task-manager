using System;
using System.Collections.Generic;
using TaskManager.Common.Validation;

namespace TaskManager.Common.Exceptions
{
    public sealed class PrecedingNotCompleteException : Exception
    {
        public PrecedingNotCompleteException()
            : base("Complete scheduled task error")
        {
            Errors = new ValidationError[] { new ("You must complete the preceding task first")};
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}