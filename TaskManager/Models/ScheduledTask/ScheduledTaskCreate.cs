namespace TaskManager.Models.ScheduledTask
{
    public class ScheduledTaskCreate
    {
        public string Email { get; set; } = "";
        public string Task { get; set; } = "";
        public string? Preceding { get; set; }
    }
}