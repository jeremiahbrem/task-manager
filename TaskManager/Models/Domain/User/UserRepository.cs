using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Database;

namespace TaskManager.Models.Domain.User
{
    public class UserRepository
    {
        private readonly TaskManagerContext _context;

        public UserRepository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUser(string email)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            return existingUser;
        }

        public async Task<List<User>> GetUsers()
        {
            var result = await _context.Set<User>()
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async System.Threading.Tasks.Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}