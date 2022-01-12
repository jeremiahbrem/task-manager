using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Exceptions;
using TaskManager.Database;
using TaskManager.Models.Domain.Task;
using Threading = System.Threading.Tasks;

namespace TaskManager.Repositories
{
    public class TaskRepository
    {
        private readonly TaskManagerContext _context;

        public TaskRepository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Threading.Task<Task?> GetTask(string name)
        {
            var existingTask = await _context.Tasks
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Name == name);

            if (existingTask == null)
            {
                throw new TaskNotFoundException(name);
            }

            return existingTask;
        }

        public async Threading.Task CheckIfExists(string name)
        {
            var existingTask = await _context.Tasks
                .FirstOrDefaultAsync(x => x.Name == name);

            if (existingTask != null)
            {
                throw new TaskAlreadyExistsException(name);
            }
        }

        public async Threading.Task<List<Task>> GetTasks()
        {
            var result = await _context.Set<Task>()
                .AsNoTracking()
                .Include(x => x.Category)
                .ToListAsync();

            return result;
        }

        public async Threading.Task<List<Task>> GetTasks(string category)
        {
            var result = await _context.Set<Task>()
                .AsNoTracking()
                .Where(x => x.Category.Name.ToLower() == category.ToLower())
                .Include(x => x.Category)
                .ToListAsync();

            return result;
        }

        public async Threading.Task AddTask(Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }
    }
}