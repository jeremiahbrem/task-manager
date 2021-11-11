namespace TaskManager.Models.Query
{
    public class Task
    {
        public string Name { get; set; } = "";

        public Category Category { get; set; } = null!;
    }
}