using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Validation;
using TaskManager.Common.Validation.ValidationModel;
using TaskManager.Database;
using TaskManager.Models.Categories;

namespace TaskManager.Models.Categories
{
    public class CategoryRepository
    {
        private readonly TaskManagerContext _context;

        public CategoryRepository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetCategory(string name)
        {
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(x => x.Name == name);

            return existingCategory;
        }

        public async Task<List<Category>> GetCategories()
        {
            var result = await _context.Set<Category>()
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async System.Threading.Tasks.Task AddCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
    }
}