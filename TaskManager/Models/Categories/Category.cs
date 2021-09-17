using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.Categories
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
    }
}