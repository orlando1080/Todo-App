using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Persistence;

public sealed class TodoRepository : ITodoRepository
{
    private readonly TodoDbContext _context;

    public TodoRepository(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem[]> GetAllAsync() =>
        await _context.Todos
            .ToArrayAsync()
            .ConfigureAwait(false);

    public async Task<TodoItem?> GetByIdAsync(Guid id) =>
        await _context.Todos
            .SingleOrDefaultAsync(todoItem => todoItem.Id == id)
            .ConfigureAwait(false);

    public async Task AddAsync(TodoItem item, CancellationToken cancellationToken) =>
        await _context.Todos
            .AddAsync(item, cancellationToken)
            .ConfigureAwait(false);


    public async Task DeleteAsync(Guid id) =>
        await _context.Todos
            .Where(todoItem => todoItem.Id == id)
            .ExecuteDeleteAsync()
            .ConfigureAwait(false);

    public async Task UpdateAsync(TodoItem item)
    {
        _context.Todos.Update(item);
    }
}