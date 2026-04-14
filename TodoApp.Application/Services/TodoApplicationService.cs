using Mapster;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Adtos;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Services;

public sealed class TodoApplicationService : ITodoApplicationService
{
    private readonly ITodoRepository _todoRepository;
    private readonly ILogger<TodoApplicationService> _logger;

    public TodoApplicationService(ITodoRepository todoRepository, ILogger<TodoApplicationService> logger)
    {
        _todoRepository = todoRepository;
        _logger = logger;
    }

    public async Task<TodoItemDto[]> GetAllAsync()
    {
        TodoItem[] todoItems = await _todoRepository.GetAllAsync().ConfigureAwait(true);

        // Manual way: items.Select(x => new TodoResponse(x.id, x.Title, x.IsCompleted)).ToArray();

        return todoItems.Adapt<TodoItemDto[]>(); // Mapster way
    }

    public Task<TodoItemDto> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTaskAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}