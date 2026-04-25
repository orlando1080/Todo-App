using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;

public interface ITodoRepository
{
    Task<TodoItem[]> GetAllAsync();
    Task<TodoItem?> GetByIdAsync(Guid id);
    Task AddAsync(TodoItem item, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(TodoItem item);
}