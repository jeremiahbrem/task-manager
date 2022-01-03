namespace TaskManager.Common.Validation
{
    public class ValidationError
    {
        public string Message { get; set; }

        public ValidationError(string message)
        {
            Message = message;
        }

        public ValidationException ToException()
        {
            return new ValidationException(this);
        }
    }
}
