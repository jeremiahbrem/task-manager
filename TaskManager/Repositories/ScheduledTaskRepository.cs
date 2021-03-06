using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Auth;
using TaskManager.Common.Exceptions;
using TaskManager.Database;
using TaskManager.Models.Domain.ScheduledTask;

namespace TaskManager.Repositories
{
    public class ScheduledTaskRepository
    {
        private readonly TaskManagerContext _context;
        private readonly CurrentUser _user;

        public ScheduledTaskRepository(TaskManagerContext context, CurrentUser user)
        {
            _context = context;
            _user = user;
        }

        public async Task<ScheduledTask?> GetScheduledTask(string id)
        {
            var existingTask = await _context.ScheduledTasks
                .Include(x => x.Task)
                .ThenInclude(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.PrecedingTask!)
                .ThenInclude(x => x.Task)
                .FirstOrDefaultAsync(x => x.ScheduledTaskId == id);

            if (existingTask == null)
            {
                throw new ScheduledTaskNotFoundException(id);
            }

            return existingTask;
        }

        public async Task<string?> GetUserEmail(string id)
        {
            var email = await _context.ScheduledTasks
                .Where(x => x.ScheduledTaskId == id)
                .Select(x => x.User.Email)
                .FirstOrDefaultAsync();

            return email;
        }

        public async Task VerifyUserEmail(string id)
        {
            var email = await _context.ScheduledTasks
                .Where(x => x.ScheduledTaskId == id)
                .Select(x => x.User.Email)
                .FirstOrDefaultAsync();

            if (email != _user.Email)
            {
                throw new ScheduledTaskUnauthorizedException(id);
            }
        }

        public async Task<List<ScheduledTask>> GetScheduledTasks()
        {
            var result = await _context.Set<ScheduledTask>()
                .AsNoTracking()
                .Where(x => x.User.Email == _user.Email)
                .Include(x => x.Task)
                .ThenInclude(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.PrecedingTask!)
                .ThenInclude(x => x.Task)
                .ToListAsync();

            return result;
        }

        public async Task<List<ScheduledTask>> GetScheduledTasks(string category)
        {
            var result = await _context.Set<ScheduledTask>()
                .AsNoTracking()
                .Where(x => x.User.Email == _user.Email
                    &&  x.Task.Category.Name.ToLower() == category.ToLower())
                .Include(x => x.Task)
                .ThenInclude(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.PrecedingTask!)
                .ThenInclude(x => x.Task)
                .ToListAsync();

            return result;
        }

        public async System.Threading.Tasks.Task AddScheduledTask(ScheduledTask scheduledTask)
        {
            _context.ScheduledTasks.Add(scheduledTask);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateScheduledTask(ScheduledTask task)
        {
            _context.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}