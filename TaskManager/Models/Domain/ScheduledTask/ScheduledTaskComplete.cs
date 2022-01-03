using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.Domain.ScheduledTask
{
    public class ScheduledTaskComplete
    {
        [Required]
        public string Id { get; set; } = "";
    }
}