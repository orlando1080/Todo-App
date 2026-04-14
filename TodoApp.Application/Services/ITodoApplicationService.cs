using TodoApp.Application.Adtos;

namespace TodoApp.Application.Services;

public interface ITodoApplicationService
{
    Task<TodoItemDto[]> GetAllAsync();
    Task<TodoItemDto> GetByIdAsync(Guid id);
    Task DeleteTaskAsync(Guid id);
}