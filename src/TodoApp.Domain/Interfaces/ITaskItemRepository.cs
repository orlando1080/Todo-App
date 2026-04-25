using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;

public interface ITaskItemRepository
{
    Task<TaskItem[]> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task AddAsync(TaskItem item, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(TaskItem item);
}