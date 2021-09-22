using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.User
{
    public class UserCreate
    {
        [Required]
        public string FirstName { get; set; } = "";
        [Required]
        public string LastName { get; set; } = "";
        [Required]
        public string Email { get; set; } = "";

        public User ToCreatedUser()
        {
            return new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
            };
        }
    }
}