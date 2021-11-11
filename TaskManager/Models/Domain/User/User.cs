namespace TaskManager.Models.Domain.User
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";

        public Query.User ToQueryObject()
        {
            return new Query.User
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email
            };
        }
    }
}