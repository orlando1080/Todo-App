using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Persistence;

public sealed class TaskItemRepository : ITaskItemRepository
{
    private readonly TodoAppDb _context;

    public TaskItemRepository(TodoAppDb context)
    {
        _context = context;
    }

    public async Task<TaskItem[]> GetAllAsync() =>
        await _context.TaskItems
            .ToArrayAsync()
            .ConfigureAwait(false);

    public async Task<TaskItem?> GetByIdAsync(Guid id) =>
        await _context.TaskItems
            .SingleOrDefaultAsync(todoItem => todoItem.Id == id)
            .ConfigureAwait(false);

    public async Task AddAsync(TaskItem item, CancellationToken cancellationToken) =>
        await _context.TaskItems
            .AddAsync(item, cancellationToken)
            .ConfigureAwait(false);


    public async Task DeleteAsync(Guid id) =>
        await _context.TaskItems
            .Where(todoItem => todoItem.Id == id)
            .ExecuteDeleteAsync()
            .ConfigureAwait(false);

    public async Task UpdateAsync(TaskItem item)
    {
        _context.TaskItems.Update(item);
    }
}