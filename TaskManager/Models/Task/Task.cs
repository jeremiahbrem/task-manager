using TaskManager.Models.Categories;

namespace TaskManager.Models.Task
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;
    }
}