using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Exceptions;
using TaskManager.Database;
using TaskManager.Models.Domain.User;

namespace TaskManager.Repositories
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

            if (existingUser == null)
            {
                throw new UserNotFoundException(email);
            }

            return existingUser;
        }

        public async Task CheckIfExists(string email)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            if (existingUser != null)
            {
                throw new UserAlreadyExistsException(email);
            }
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