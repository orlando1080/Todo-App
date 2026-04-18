using Mapster;
using TodoApp.Application.Dtos;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.TodoTasks.Queries;

public sealed class GetTaskByIdQueryHandler
{
    private readonly ITodoRepository _todoRepository;

    public GetTaskByIdQueryHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<TodoItemDto?> HandleAsync(GetTaskByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        TodoItem? todoItem = await _todoRepository.GetByIdAsync(query.Id).ConfigureAwait(false);

        return todoItem?.Adapt<TodoItemDto>();
    }
}