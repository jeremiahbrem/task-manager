using System;
using System.Collections.Generic;

namespace TaskManager.Common.Validation
{
    public class ValidationResponse
    {
        public string Message { get; set; } = "";
        public IEnumerable<ValidationError>? Errors { get; set; } = Array.Empty<ValidationError>();
    }
}