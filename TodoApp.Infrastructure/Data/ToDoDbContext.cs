using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Data;

// This class is the bridge between the code and the database.
public class TodoDbContext: DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    // This represents the "Table" in SQL
    public DbSet<TodoItem> Todos =>
        Set<TodoItem>();
}