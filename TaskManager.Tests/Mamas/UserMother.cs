using TaskManager.Models.User;

namespace TaskManager.Tests.Mamas
{
    public class UserMother : IntegrationTestBase
    {
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _email;
        public User User => CreateUser();

        public UserMother(string firstName, string lastName, string email)
        {
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
        }

        private User CreateUser()
        {
            return new User
            {
                FirstName = _firstName,
                LastName = _lastName,
                Email = _email
            };
        }
    }
}