using System;
using TaskManager.Models.Domain.Categories;

namespace TaskManager.Models.Domain.Task
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public Query.Task ToQueryObject()
        {
            return new Query.Task
            {
                Name = Name,
                Category = Category.Name
            };
        }
    }
}