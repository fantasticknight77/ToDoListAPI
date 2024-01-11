using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data.Models;

namespace ToDoListAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<ToDoItem> ToDoItems { get; set; }

        public virtual DbSet<ToDoItemTag> ToDoItemTags { get; set; }

        public virtual DbSet<ToDoItemUser> ToDoItemUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ToDoItemUser>(entity =>
            {
                entity.HasIndex(m => new { m.ToDoItemID, m.UserID }).IsUnique();
            });

            builder.Entity<ToDoItemTag>(entity =>
            {
                entity.HasIndex(m => new { m.ToDoItemID, m.Name }).IsUnique();
            });

            base.OnModelCreating(builder);
        }
    }
}
