using System.Collections.Generic;

namespace TaskManager.Models.User
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public ICollection<ScheduledTask.ScheduledTask> Tasks { get; set; } = new List<ScheduledTask.ScheduledTask>();
    }
}