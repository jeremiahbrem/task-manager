using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Database;

namespace TaskManager.Models.Domain.ScheduledTask
{
    public class ScheduledTaskRepository
    {
        private readonly TaskManagerContext _context;

        public ScheduledTaskRepository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<ScheduledTask?> GetScheduledTask(string id)
        {
            var existingTask = await _context.ScheduledTasks

                .Include(x => x.Task)
                .Include(x => x.User)
                .Include(x => x.PrecedingTask)
                .FirstOrDefaultAsync(x => x.ScheduledTaskId == id);

            return existingTask;
        }

        public async Task<List<ScheduledTask>> GetScheduledTasks()
        {
            var result = await _context.Set<ScheduledTask>()
                .AsNoTracking()
                .Include(x => x.Task)
                .Include(x => x.User)
                .Include(x => x.PrecedingTask)
                .ToListAsync();

            return result;
        }

        public async System.Threading.Tasks.Task AddScheduledTask(ScheduledTask scheduledTask)
        {
            _context.ScheduledTasks.Add(scheduledTask);
            await _context.SaveChangesAsync();
        }
    }
}