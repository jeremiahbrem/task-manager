using Microsoft.EntityFrameworkCore;
using TaskManager.Models.Domain.Categories;
using TaskManager.Models.Domain.ScheduledTask;
using TaskManager.Models.Domain.Task;
using TaskManager.Models.Domain.User;

namespace TaskManager.Database
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; } = null!;
        public DbSet<ScheduledTask> ScheduledTasks { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Task>()
                .HasIndex(x => x.Name)
                .IsUnique();

            builder.Entity<Category>()
                .HasIndex(x => x.Name)
                .IsUnique();

            builder.Entity<ScheduledTask>()
                .HasIndex(x => x.ScheduledTaskId)
                .IsUnique();
        }

    }
}