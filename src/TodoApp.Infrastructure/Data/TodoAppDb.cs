using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Data;

// This class is the bridge between the code and the database.
public class TodoAppDb: DbContext
{
    public TodoAppDb(DbContextOptions<TodoAppDb> options) : base(options)
    {
    }

    // This represents the "Table" in SQL
    public DbSet<TaskItem> TaskItems =>
        Set<TaskItem>();
}