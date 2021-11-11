using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.Domain.Categories
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";

        public Query.Category ToQueryObject()
        {
            return new Query.Category
            {
                Name = Name
            };
        }
    }
}