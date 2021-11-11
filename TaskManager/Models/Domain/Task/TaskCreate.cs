using System.ComponentModel.DataAnnotations;
using TaskManager.Models.Domain.Categories;

namespace TaskManager.Models.Domain.Task
{
    public class TaskCreate
    {
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public string Category { get; set; } = "";

        public Task ToCreatedTask(Category category)
        {
            return new Task
            {
                Name = Name,
                Category = category,
            };
        }
    }
}