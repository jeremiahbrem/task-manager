using System.ComponentModel.DataAnnotations;

namespace TaskManager.Commands
{
    public class ScheduledTaskComplete
    {
        [Required]
        public string Id { get; set; } = "";
    }
}