using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;

public interface ITaskItemRepository
{
    Task<TaskItem[]> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task Add(TaskItem item, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id);
    Task Update(TaskItem item);
}