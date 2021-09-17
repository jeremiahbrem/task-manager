using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Validation;
using TaskManager.Common.Validation.ValidationModel;
using TaskManager.Database;

namespace TaskManager.Models.Task
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
                .FirstOrDefaultAsync(x => x.Name == name);

            return existingTask;
        }

        public async System.Threading.Tasks.Task AddTask(Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }
    }
}