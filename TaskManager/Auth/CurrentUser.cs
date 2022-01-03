namespace TaskManager.Auth
{
    public sealed class CurrentUser
    {
        public CurrentUser(string? email)
        {
            Email = email;
        }

        public string? Email { get; }
    }
}