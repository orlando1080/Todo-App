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

    public Task Add(TaskItem item, CancellationToken cancellationToken)
    {
        _context.TaskItems.Add(item);
        return Task.CompletedTask;
    }

    public async Task<bool> DeleteAsync(Guid id) =>
        await _context.TaskItems
            .Where(todoItem => todoItem.Id == id)
            .ExecuteDeleteAsync()
            .ConfigureAwait(false) > 0;

    public Task Update(TaskItem item)
    {
        _context.TaskItems.Update(item);
        return Task.CompletedTask;
    }
}