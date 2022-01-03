using System.Text.Json.Serialization;

namespace TaskManager.Models.Query
{
    public class ScheduledTask
    {
        [JsonInclude]
        public string Id = null!;
        public Task Task { get; set; } = null!;
        public User User { get; set; } = null!;
        public bool Completed { get; set; }
        public string? Preceding { get; set; }
        public string? PrecedingId { get; set; }
    }
}