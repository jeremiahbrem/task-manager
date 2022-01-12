using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Exceptions;
using TaskManager.Database;
using TaskManager.Models.Domain.Categories;

namespace TaskManager.Repositories
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

            if (existingCategory == null)
            {
                throw new CategoryNotFoundException(name);
            }

            return existingCategory;
        }

        public async Task CheckIfExists(string name)
        {
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(x => x.Name == name);

            if (existingCategory != null)
            {
                throw new CategoryAlreadyExistsException(name);
            }
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