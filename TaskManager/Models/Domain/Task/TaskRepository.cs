using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Database;

namespace TaskManager.Models.Domain.Task
{
    public class TaskRepository
    {
        private readonly TaskManagerContext _context;

        public TaskRepository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<Task?> GetTask(string name)
        {
            var existingTask = await _context.Tasks
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Name == name);

            return existingTask;
        }

        public async Task<List<Task>> GetTasks()
        {
            var result = await _context.Set<Task>()
                .AsNoTracking()
                .Include(x => x.Category)
                .ToListAsync();

            return result;
        }

        public async System.Threading.Tasks.Task AddTask(Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }
    }
}